namespace Hospital_Management_System.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; } = "Scheduled";

        public DateTime? CancelledAt { get; set; }

        // Domain logic
        public int GetFutureDays()
        {
            return (AppointmentDate - DateTime.Now).Days;
        }

        public bool IsPast()
        {
            return AppointmentDate < DateTime.Now;
        }
    }

}
