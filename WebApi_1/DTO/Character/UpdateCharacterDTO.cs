using WebApi_1.Models;

namespace WebApi_1.DTO.Character
{
    public class UpdateCharacterDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }

        public RpgClass Class { get; set; }
    }
}