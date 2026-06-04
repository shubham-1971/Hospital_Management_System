using Hospital_Management_System.Data.Repositories.Interfaces;
using Hospital_Management_System.DTOs;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public void AddAppointment(CreateAppointmentDto dto)
        {
            if (dto.AppointmentDate < DateTime.Now)
                throw new ArgumentException("Appointment cannot be in the past");

            try
            {
                _repo.AddAppointment(dto.PatientId, dto.DoctorId, dto.AppointmentDate);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50010)
                    throw new KeyNotFoundException("Patient not found");

                if (ex.Number == 50011)
                    throw new KeyNotFoundException("Doctor not found");

                if (ex.Number == 50012)
                    throw new InvalidOperationException("Doctor not available");

                if (ex.Number == 50015)
                    throw new InvalidOperationException("Doctor already booked");

                throw;
            }
        }

        public void CancelAppointment(int id) =>
            _repo.CancelAppointment(id);

        public List<Appointment> GetUpcomingAppointments() =>
            _repo.GetUpcomingAppointments();

        public List<Appointment> GetAppointmentsByDoctor(int doctorId) =>
            _repo.GetAppointmentsByDoctor(doctorId);

        public List<object> GetAppointmentDetails() =>
            _repo.GetAppointmentDetails();

        public List<object> GetDoctorsWithMoreAppointments() =>
            _repo.GetDoctorsWithMoreAppointments();

        public List<object> GetRevenueBySpecialization() =>
            _repo.GetRevenueBySpecialization();

        public List<object> GetDuplicateAppointments() =>
            _repo.GetDuplicateAppointments();

        public List<object> GetNext7DaysAppointments() =>
            _repo.GetNext7DaysAppointments();
    }
}
