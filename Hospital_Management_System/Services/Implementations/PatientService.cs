using Hospital_Management_System.Data.Repositories.Interfaces;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services.Interfaces;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repo;

    public PatientService(IPatientRepository repo)
    {
        _repo = repo;
    }

    public List<Patient> GetActivePatients() => _repo.GetActivePatients();

    public void AddPatient(Patient p) => _repo.AddPatient(p);

    public void UpdatePatient(Patient p) => _repo.UpdatePatient(p);

    public void DeactivatePatient(int id) => _repo.DeactivatePatient(id);
}