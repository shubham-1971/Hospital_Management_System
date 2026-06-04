namespace Hospital_Management_System.Services.Interfaces
{
    using Hospital_Management_System.Models;

    public interface IDoctorService
    {
        List<Doctor> GetDoctors(string? specialization, bool? available);
    }
}
