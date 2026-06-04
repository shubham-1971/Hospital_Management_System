using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        [RegularExpression("Male|Female|Other")]
        public string Gender { get; set; }

        [Required]
        [StringLength(15)]
        public string Mob { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Status { get; set; } = "Active";

        // AGE PROPERTY (CALCULATED)
        public int Age
        {
            get
            {
                if (Dob == default)
                    return 0;

                var today = DateTime.Today;
                int age = today.Year - Dob.Year;

                // Adjust if birthday not yet occurred this year
                if (Dob.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

    }
}
