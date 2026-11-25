using System.ComponentModel.DataAnnotations;

namespace GameApi.DTOs
{
    public class CreateCharacterRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Class { get; set; } = string.Empty;
    }
}
