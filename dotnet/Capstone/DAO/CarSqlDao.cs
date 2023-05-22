using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class CarSqlDao : ICarSqlDao
    {
        private readonly string connectionString;

        public CarSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Car AddNewCar(Car car)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"insert into cars(licensePlate,make,model,vin,color)
                    values(@licensePlate,@make,@model,@vin,@color)", connection);
                    cmd.Parameters.AddWithValue("@licensePlate",car.LicensePlate);
                    cmd.Parameters.AddWithValue("@make",car.Make);
                    cmd.Parameters.AddWithValue("@model",car.Model);
                    cmd.Parameters.AddWithValue("@vin",car.VIN);
                    cmd.Parameters.AddWithValue("@color",car.Color);
                    cmd.ExecuteNonQuery();

                    return GetCar(car.LicensePlate);
                }
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong while adding a new car: " + e);
            }
        }
        public List<Car> ReturnAllCarsList()
        {
            List<Car> listToReturn = new List<Car>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM cars", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Car car = GetCarFromReader(reader);
                        listToReturn.Add(car);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception occurred while getting all cars in a list: {e}");
            }

            return listToReturn;
        }
        public Car GetCar(string licensePlate)
        {
            Car returnCar = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM cars WHERE licensePlate = @plate;", conn);
                    cmd.Parameters.AddWithValue("@plate", licensePlate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return returnCar = GetCarFromReader(reader);
                    }
                }
            }
            catch (SqlException e)
            {

                Console.WriteLine($"An exception happened while getting car by id in the DAO: {e}");
            }
            return returnCar;
        }
        public List<Car> CarToPickUp()
        {
            Car carToReturn = null;
            List<Car> results = new List<Car>();
            try
            {                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select parkingSpotId,al.ticketNumber, needsPickedUp,
                      checkIn, checkOut, c.licensePlate, make, model, color, vin
                      from activeLot as al
                      join transactions as t on t.ticketNumber = al.ticketNumber
                      join owners_cars as oc on oc.id = t.owner_carsId
                      join cars as c on c.licensePlate = oc.licensePlate", conn);
                    // Pretty sure the logic to return the first car that needs picked up
                    // can all be handled with SQL...
                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        carToReturn = GetCarFromReader(reader);
                        results.Add(carToReturn);
                    }
                }
            }
            catch (Exception e)
            { 
                Console.WriteLine($"An exception happened while getting a car to pickup: {e}");
            }
            return results;
        }
        public bool AddCarToPickUp(int ticket)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE activeLot " +
                        "SET needsPickedUp = 1 " +
                        "WHERE ticketNumber = @ticketNumber;", conn);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticket);
                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"An exception happened while adding a car to pickup: {e}");
                return false;
            }
            

        }
        private Car GetCarFromReader(SqlDataReader reader)
        {
            Car c = new Car()
            {
                LicensePlate = Convert.ToString(reader["licensePlate"]),
                Make = Convert.ToString(reader["make"]),
                Model = Convert.ToString(reader["model"]),
                VIN = Convert.ToString(reader["vin"]),
                Color = Convert.ToString(reader["color"])
            };
            try
            {
                c.needsPickedUp = Convert.ToBoolean(reader["needsPickedUp"]);
            }
            catch (Exception)
            {
                c.needsPickedUp = false;
            }

            return c;
        }
    }
}
