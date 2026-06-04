using System.Data;
using Microsoft.Data.SqlClient;
using Hospital_Management_System.Data.Repositories.Interfaces;
using Hospital_Management_System.Models;

public class PatientRepository : IPatientRepository
{
    private readonly string _connectionString;

    public PatientRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    // Add patient method
    public void AddPatient(Patient patient)
    {
        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("sp_AddPatient", con);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@fullName", patient.FullName);
        cmd.Parameters.AddWithValue("@dob", patient.Dob);
        cmd.Parameters.AddWithValue("@gender", patient.Gender);
        cmd.Parameters.AddWithValue("@mob", patient.Mob);
        cmd.Parameters.AddWithValue("@email", (object?)patient.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@status", patient.Status);

        con.Open();
        cmd.ExecuteNonQuery();
    }
    // Get all patients method
    public List<Patient> GetActivePatients()
    {
        List<Patient> list = new();

        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("sp_GetActivePatients", con);

        cmd.CommandType = CommandType.StoredProcedure;
        con.Open();

        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Patient
            {
                Id = (int)reader["id"],
                FullName = reader["fullName"].ToString(),
                Dob = Convert.ToDateTime(reader["dob"]),
                Gender = reader["gender"].ToString(),
                Mob = reader["mob"].ToString(),
                Email = reader["email"] == DBNull.Value ? null : reader["email"].ToString(),
                Status = reader["status"].ToString()
            });
        }

        return list;
    }
    // Update patient method
    public void UpdatePatient(Patient patient)
    {
        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("sp_UpdatePatient", con);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@id", patient.Id);
        cmd.Parameters.AddWithValue("@fullName", patient.FullName);
        cmd.Parameters.AddWithValue("@dob", patient.Dob);
        cmd.Parameters.AddWithValue("@gender", patient.Gender);
        cmd.Parameters.AddWithValue("@mob", patient.Mob);
        cmd.Parameters.AddWithValue("@email", (object?)patient.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@status", patient.Status);

        con.Open();
        cmd.ExecuteNonQuery();
    }
    // soft Delete - Deactivate patient method
    public void DeactivatePatient(int id)
    {
        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("sp_DeactivatePatient", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);

        con.Open();
        cmd.ExecuteNonQuery();
    }
}