using WebApi_1.Data;

namespace WebApi_1.Models
{
    public class Character
    {

        public int Id { get; set; }
        public string Name { get; set; } = "Player";
        public int HP { get; set; } = 100;
        public int MP { get; set; } = 10;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;

        public RpgClass Class { get; set; } = RpgClass.Adventurer;

        public User CharacterUser { get; set; }



    }
}