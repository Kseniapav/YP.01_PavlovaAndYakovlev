using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsYchet
{
    public class MasterRepository: IMasterRepository
    {
        private SqlConnection connection;

        public MasterRepository(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public List<Master> GetAllMasters()
        {
            List<Master> masters = new List<Master>();

            try
            {
                connection.Open();
                string query = "SELECT IdMaster, Name, Speciality, Number FROM Masters";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Master master = new Master
                        {
                            IdMaster = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Speciality = reader.GetString(2),
                            Number = reader.GetString(3)
                        };
                        masters.Add(master);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке мастеров: {ex.Message}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return masters;
        }
    }
}
