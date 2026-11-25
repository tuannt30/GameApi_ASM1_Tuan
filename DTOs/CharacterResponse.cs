namespace GameApi.DTOs
{
    public class CharacterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Hp { get; set; }
        public int AccountId { get; set; }
    }
}
