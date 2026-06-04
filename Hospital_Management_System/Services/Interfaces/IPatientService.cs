using Hospital_Management_System.Models;

namespace Hospital_Management_System.Services.Interfaces
{
    public interface IPatientService
    {
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeactivatePatient(int id);
        List<Patient> GetActivePatients();
    }

}
