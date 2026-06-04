namespace Hospital_Management_System.Data.Repositories.Implementations
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Hospital_Management_System.Data.Repositories.Interfaces;
    using Hospital_Management_System.Models;

    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        // Method to filter doctors on specialization and availability (Both can be null)
        public List<Doctor> GetDoctors(string? specialization, bool? available)
        {
            List<Doctor> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetDoctorsByFilter", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@specialization",
                string.IsNullOrEmpty(specialization) ? DBNull.Value : specialization);

            cmd.Parameters.AddWithValue("@available",
                available.HasValue ? available : DBNull.Value);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Doctor
                {
                    Id = (int)reader["id"],
                    FullName = reader["fullName"].ToString(),
                    Specialization = reader["specialization"].ToString(),
                    Mob = reader["mob"].ToString(),
                    Fee = (int)reader["fee"],
                    Available = (bool)reader["available"]
                });
            }

            return list;
        }
    }
}
