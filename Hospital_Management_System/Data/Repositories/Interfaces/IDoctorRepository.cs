namespace Hospital_Management_System.Data.Repositories.Interfaces
{

    using Hospital_Management_System.Models;

    public interface IDoctorRepository
    {
        List<Doctor> GetDoctors(string? specialization, bool? available);
    }

}
