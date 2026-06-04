using Hospital_Management_System.DTOs;
using Hospital_Management_System.Models;

namespace Hospital_Management_System.Services.Interfaces
{
    public interface IAppointmentService
    {
        void AddAppointment(CreateAppointmentDto dto);
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
