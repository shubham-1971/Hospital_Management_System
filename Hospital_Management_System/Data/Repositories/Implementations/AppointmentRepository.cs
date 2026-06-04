using System.Data;
using Microsoft.Data.SqlClient;
using Hospital_Management_System.Data.Repositories.Interfaces;
using Hospital_Management_System.Models;

namespace Hospital_Management_System.Data.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ADD
        public void AddAppointment(int patientId, int doctorId, DateTime appointmentDate)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddAppointment", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@patientId", patientId);
            cmd.Parameters.AddWithValue("@doctorId", doctorId);
            cmd.Parameters.AddWithValue("@appointmentDate", appointmentDate);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // CANCEL
        public void CancelAppointment(int appointmentId)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_CancelAppointment", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // UPCOMING
        public List<Appointment> GetUpcomingAppointments()
        {
            List<Appointment> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetUpcomingAppointments", con);

            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Appointment
                {
                    AppointmentId = Convert.ToInt32(reader["appointmentId"]),
                    PatientId = Convert.ToInt32(reader["patientId"]),
                    DoctorId = Convert.ToInt32(reader["doctorId"]),
                    AppointmentDate = Convert.ToDateTime(reader["appointmentDate"]),
                    Status = reader["status"].ToString()
                });
            }

            return list;
        }

        // BY DOCTOR
        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            List<Appointment> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAppointmentsByDoctor", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@doctorId", doctorId);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Appointment
                {
                    AppointmentId = Convert.ToInt32(reader["appointmentId"]),
                    PatientId = Convert.ToInt32(reader["patientId"]),
                    DoctorId = Convert.ToInt32(reader["doctorId"]),
                    AppointmentDate = Convert.ToDateTime(reader["appointmentDate"]),
                    Status = reader["status"].ToString()
                });
            }

            return list;
        }

        // DETAILS
        public List<object> GetAppointmentDetails()
        {
            var list = new List<object>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAppointmentDetails", con);

            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new
                {
                    PatientName = reader["PatientName"]?.ToString(),
                    DoctorName = reader["DoctorName"]?.ToString(),
                    Specialization = reader["Specialization"]?.ToString(),
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                    Status = reader["Status"]?.ToString(),
                    Fee = Convert.ToInt32(reader["Fee"])
                });
            }

            return list;
        }

        // DOCTORS WITH MORE APPOINTMENTS
        public List<object> GetDoctorsWithMoreAppointments()
        {
            List<object> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetDoctorsWithMoreAppointments", con);

            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string row =
                    reader[0].ToString() + " - " +
                    reader[1].ToString();

                list.Add(row);
            }

            return list;
        }

        // REVENUE
        public List<object> GetRevenueBySpecialization()
        {
            List<object> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetRevenueBySpecialization", con);

            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string row =
                    reader[0].ToString() + " : " +
                    reader[1].ToString();

                list.Add(row);
            }

            return list;
        }

        // DUPLICATES - same time, same doctor
        public List<object> GetDuplicateAppointments()
        {
            List<object> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetDuplicateAppointments", con);

            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string row =
                    reader["patientId"].ToString() + " - " +
                    reader["appointmentDate"].ToString();

                list.Add(row);
            }

            return list;
        }

        // NEXT 7 DAYS
        public List<object> GetNext7DaysAppointments()
        {
            List<object> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetNext7DaysAppointments", con);

            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string row =
                    reader["appointmentId"].ToString() + " | " +
                    reader["appointmentDate"].ToString();

                list.Add(row);
            }

            return list;
        }
    }
}
