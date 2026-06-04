using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Required]
        public string Mob { get; set; }

        [Range(0, int.MaxValue)]
        public int Fee { get; set; }

        public bool Available { get; set; } = true;
    }
}
