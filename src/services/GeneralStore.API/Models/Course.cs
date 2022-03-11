using System.ComponentModel.DataAnnotations;

namespace GeneralStore.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
