using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi_1.Models;

namespace WebApi_1.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;


        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var user = await _context.Users.FirstAsync(x => x.Username.ToLower() == username.ToLower());
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Incorrect Password";
                    return serviceResponse;

                }

                serviceResponse.Data = CreateToken(user);
                return serviceResponse;
            }
            catch (Exception ex)
            {

                serviceResponse.Success = false;
                serviceResponse.Message = "Username not found!";
                return serviceResponse;
            }


        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var serviceResponse = new ServiceResponse<int>();

            if (await UserExists(user.Username))
            {

                serviceResponse.Success = false;
                serviceResponse.Message = "User already Exists";
                return serviceResponse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            serviceResponse.Data = user.UserId;
            return serviceResponse;


        }

        public async Task<bool> UserExists(string username)
        {
            if (_context.Users.Count() > 0 && await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }

                return true;
            }
        }

        private string CreateToken(User user)
        {
            //create the claims
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            //create the secret security key 

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));


            //signing signature
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}