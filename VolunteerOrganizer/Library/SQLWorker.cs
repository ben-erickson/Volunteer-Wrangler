using System.Data;
using Microsoft.Data.SqlClient;

namespace VolunteerOrganizer.Library
{
    static public class SQLWorker
    {
        private static string _connectionString = "Server=LAPTOP-HS9E422S;Database=VolunteerOrganizer;Trusted_Connection=True;TrustServerCertificate=True;";

        public static DataTable ExecuteQuery(SqlCommand command)
        {
            DataTable queryResult = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();

                queryResult.Load(reader);

                reader.Close();
            }

            return queryResult;
        }

        public static void ExecuteNonQuery(SqlCommand command)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                command.Connection = connection;

                command.ExecuteNonQuery();
            }
        }
    }
}
