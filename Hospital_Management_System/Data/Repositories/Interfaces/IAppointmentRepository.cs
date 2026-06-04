using Hospital_Management_System.Models;

namespace Hospital_Management_System.Data.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        void AddAppointment(int patientId, int doctorId, DateTime appointmentDate);
        void CancelAppointment(int appointmentId);
        List<Appointment> GetUpcomingAppointments();
        List<Appointment> GetAppointmentsByDoctor(int doctorId);


        List<object> GetAppointmentDetails();
        List<object> GetDoctorsWithMoreAppointments();
        List<object> GetRevenueBySpecialization();
        List<object> GetDuplicateAppointments();
        List<object> GetNext7DaysAppointments();

    }
}
