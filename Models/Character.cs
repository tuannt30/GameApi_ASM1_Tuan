using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameApi.Models
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Class { get; set; } = string.Empty;

        public int Level { get; set; } = 1;

        public int Hp { get; set; } = 100;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
    }
}
