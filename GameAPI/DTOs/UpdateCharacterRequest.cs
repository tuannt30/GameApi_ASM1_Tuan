using System.ComponentModel.DataAnnotations;

namespace GameApi.DTOs
{
    public class UpdateCharacterRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Class { get; set; } = string.Empty;

        [Required]
        [Range(1, 100)]
        public int? Level { get; set; }

        [Required]
        [Range(0, 100000)]
        public int? Hp { get; set; }
    }
}
