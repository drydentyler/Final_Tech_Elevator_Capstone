using System;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class Owners_CarsDAO : IOwners_Cars
    {
        private readonly string connectionString;

        public Owners_CarsDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public int AddNewRecordOwnersCars(int customerId, string licensePlate)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"insert into owners_cars(customerId,licensePlate)
                    output inserted.id
                    values(@customerId,@licensePlate)", connection);
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@licensePlate", licensePlate);
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong adding a new record: " + e);
            }
        }
    }
}
