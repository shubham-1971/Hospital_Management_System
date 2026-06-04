using Hospital_Management_System.Models;

namespace Hospital_Management_System.Data.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        List<Patient> GetActivePatients();
        void DeactivatePatient(int id);
    }

}
