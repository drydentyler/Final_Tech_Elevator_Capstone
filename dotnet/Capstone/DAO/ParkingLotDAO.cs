using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class ParkingLotDAO : IParkingLot
    {
        private readonly string connectionString;
        public ParkingLotDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public SeeDetailsDTO SeeSpotDetails(int parkingSpotNumber)
        {
            try
            {
                SeeDetailsDTO dto = new SeeDetailsDTO();
                Customer customer = new Customer();

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select parkingSpotId, t.ticketNumber, firstName,lastName, phoneNumber from activeLot as al
                      join transactions as t on t.ticketNumber = al.ticketNumber
                      join owners_cars as oc on oc.id = t.owner_carsId
                      join customers as c on c.customerId = oc.customerId
                      where parkingSpotId = @parkingSpotNumber", connection);
                    cmd.Parameters.AddWithValue("@parkingSpotNumber", parkingSpotNumber);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        customer.FirstName = (string)reader["firstName"];
                        customer.LastName = (string)reader["lastName"];
                        customer.PhoneNumber = (string)reader["phoneNumber"];
                        dto.customer = customer;
                        dto.ticketNumber = (int)reader["ticketNumber"];
                    }
                }
                return dto;
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong getting spot details: " + e);
            }
        }

        public int GetParkingSpotByLicensePlate(string licesnsePlate)
        {
            try
            {
                int parkingSpot;
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select parkingSpotId from owners_cars as oc
                    join transactions as t on t.owner_carsId = oc.id
                    join activeLot as al on al.ticketNumber = t.ticketNumber
                    where licensePlate = @licensePlate", connection);
                    cmd.Parameters.AddWithValue("@licensePlate", licesnsePlate);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return (int)reader["parkingSpotId"];
                    }
                }
                return -1;
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong retrieving the parking spot by license palte: " + e);
            }
        }
        public List<int> ListOfAvailableSpots()
        {
            List<int> results = new List<int>();
            try
            { 
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select parkingSpotId from activeLot
                    where ticketNumber is null", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int placeholder = (int)reader["parkingSpotId"];
                        results.Add(placeholder);
                    }
                }
                return results;
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong getting the list of available spots: " + e);
            }
        }
        public bool CarIsCurrentlyParkedInLot(int ticketNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select * from activeLot
                    where ticketNumber = @ticketNumber", connection);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int? readerNumber = (int)reader["ticketNumber"];
                        return readerNumber != null;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public bool RemoveCarFromParkingLot(int ticketNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"update activeLot
                    set ticketNumber = null, needsPickedUp = null
                    where ticketNumber = @ticketNumber", connection);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                    cmd.ExecuteNonQuery();

                    return CarIsCurrentlyParkedInLot(ticketNumber);
                }
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public bool AddCarToParkingLot(int parkingSpot, int ticketNumber)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"update activeLot
                      set ticketNumber = @ticketNumber, needsPickedUp = 0
                      where parkingSpotId = @parkingSpotId", connection);
                    cmd.Parameters.AddWithValue("@parkingSpotId", parkingSpot);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public List<Car> GetListOfCarsCurrentlyInTheLot()
        {
            try
            {
                List<Car> listOfCarsInLot = new List<Car>();
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select parkingSpotId,al.ticketNumber, needsPickedUp,
                      checkIn, checkOut, c.licensePlate, make, model, color, vin
                      from activeLot as al
                      join transactions as t on t.ticketNumber = al.ticketNumber
                      join owners_cars as oc on oc.id = t.owner_carsId
                      join cars as c on c.licensePlate = oc.licensePlate", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Car placeholder = new Car();
                        placeholder.LicensePlate = Convert.ToString(reader["licensePlate"]);
                        placeholder.Make = (string)(reader["make"]);
                        placeholder.Model = (string)(reader["model"]);
                        placeholder.VIN = (string)(reader["vin"]);
                        placeholder.Color = (string)(reader["color"]);
                        placeholder.needsPickedUp = Convert.ToBoolean(reader["needsPickedUp"]);
                        listOfCarsInLot.Add(placeholder);
                    }
                }
                return listOfCarsInLot;
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong");
            }
        }
        public int NumberOfSpotsAvailableInLot()
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select count(*) from activeLot
                    where ticketNumber is null", connection);
                    int numberOfSpots = (int)cmd.ExecuteScalar();

                    return numberOfSpots;
                }

                return 0;
            }
            catch(Exception e)
            {
                throw new Exception("something went wrong");
            }
        }
        public bool ToggleCarPickUpStatus(int ticketNumber)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE activeLot 
                    SET needsPickedUp = 1
                    where ticketNumber = @ticketNumber; ", connection);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public void RemoveCarFromParkingLot(string licensePlate, int valetId)
        {
            try
            {
                int placeholder = 0;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT ticketNumber FROM transactions as t
                    JOIN owners_cars as oc ON oc.id = t.owner_carsId
                    WHERE oc.licensePlate = @licensePlate
                    order by ticketNumber desc;", conn);
                    cmd.Parameters.AddWithValue("@licensePlate", licensePlate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        placeholder = Convert.ToInt32(reader["ticketNumber"]);
                    }
                    conn.Close();
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand("UPDATE activeLot " +
                        "SET ticketNumber = NULL, needsPickedUp = NULL " +
                        "WHERE ticketNumber = @placeholder", conn);
                    cmd2.Parameters.AddWithValue("@placeholder", placeholder);
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd3 = new SqlCommand("UPDATE transactions " +
                    "SET checkOut = @currentTime, valetPickingUp = @userId " +
                    "WHERE ticketNumber = @placeholder", conn);
                    cmd3.Parameters.AddWithValue("@userId", valetId);
                    cmd3.Parameters.AddWithValue("@currentTime", DateTime.UtcNow);
                    cmd3.Parameters.AddWithValue("@placeholder", placeholder);

                    cmd3.ExecuteNonQuery();
                    conn.Close();
                } 
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
