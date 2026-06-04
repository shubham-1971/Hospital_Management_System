namespace Hospital_Management_System.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CreatePatientDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Mob { get; set; }

        public string Email { get; set; }

        public string Status { get; set; } = "Active";
    }

}
