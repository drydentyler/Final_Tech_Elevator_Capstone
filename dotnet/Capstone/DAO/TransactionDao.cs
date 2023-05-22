using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class TransactionDao : ITransaction
    {

        private readonly string connectionString;
        public TransactionDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public List<Transaction> GetAllTransactions()
        {
            try
            {
                List<Transaction> listOfTransactions = new List<Transaction>();
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select * from transactions", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transaction placeholder = BuildTransaction(reader);
                        listOfTransactions.Add(placeholder);
                    }
                }
                return listOfTransactions;
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong");
            }
        }
        public Transaction GetTransactionByTicketNumber(int ticketNumber)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select * from transactions 
                    where ticketNumber = @ticketNumber", connection);
                    cmd.Parameters.AddWithValue("@ticketNumber",ticketNumber);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return BuildTransaction(reader);
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }
        public Transaction AddANewTransaction(int valetId, int owner_carsId)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(@"insert into transactions(amountPaid,owner_carsId,checkIn,valetDroppingOff)
                    output inserted.ticketNumber
                    values(0,@owner_carsId,@checkIn,@valetDroppingOff);", connection);
                    cmd.Parameters.AddWithValue("@owner_carsId", owner_carsId);
                    cmd.Parameters.AddWithValue("@checkIn",DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@valetDroppingOff",valetId);
                    int ticketNumber = (int)cmd.ExecuteScalar();
                    return GetTransactionByTicketNumber(ticketNumber);
                }
            }
            catch(Exception e)
            {
                throw new Exception("Something went wrong adding a new transaction: " + e);
            }
        }
        public Transaction AddNewTransaction(Transaction transaction)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    //BEHOLD THE MOST DISGUSTING LINE OF CODE I HAVE PROBABLY EVER WRITTEN
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"insert into transactions(amountPaid,owner_carsId,checkIn,"+$"{(transaction.CheckOut != null ? "checkOut," : "")}"+"valetDroppingOff"+$"{(transaction.ValetIdPickingUp != null ? ",valetPickingUp" : "")}"+") "+
                   "output inserted.ticketNumber "+
                    "values(@amountPaid,@owner_carsId,@checkIn,"+$"{(transaction.CheckOut != null ? "@checkOut," : "")}"+"@valetDroppingOffId"+$"{(transaction.ValetIdPickingUp != null ? ",@valetPickingUpId" : "")}"+")", connection);
                    cmd.Parameters.AddWithValue("@amountPaid", transaction.AmountPaid);
                    cmd.Parameters.AddWithValue("@owner_carsId", transaction.Owner_carId);
                    cmd.Parameters.AddWithValue("@checkIn", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@valetDroppingOffId", transaction.ValetIdDroppingOff);

                    if(transaction.ValetIdPickingUp != null)
                    {
                        cmd.Parameters.AddWithValue("@valetPickingUpId", transaction.ValetIdPickingUp);
                    };
                    if(transaction.CheckOut != null)
                    {
                        cmd.Parameters.AddWithValue("@checkOut", transaction.CheckOut);
                    };
                    int ticketNumber = (int)cmd.ExecuteScalar();
                    return GetTransactionByTicketNumber(ticketNumber);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Transaction UpdateTransaction(Transaction transaction)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"update transactions
                    set amountPaid = @newAmount, checkOut = @checkOut, valetPickingUp = @valetPickingUp
                    where ticketNumber = @ticketNumber", connection);
                    cmd.Parameters.AddWithValue("@newAmount", transaction.AmountPaid);
                    cmd.Parameters.AddWithValue("@checkOut", transaction.CheckOut);
                    cmd.Parameters.AddWithValue("@valetPickingUp",transaction.ValetIdPickingUp);
                    cmd.Parameters.AddWithValue("@ticketNumber", transaction.TicketNumber);
                    cmd.ExecuteNonQuery();

                    return GetTransactionByTicketNumber(transaction.TicketNumber);
                }

            }
            catch (Exception)
            {
                throw new Exception("something went wrong");
            }

        }

        public string GetTimeParked(int ticketNumber)
        {
            DateTime start = DateTime.UtcNow;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT checkIn " +
                        "FROM transactions " +
                        "WHERE ticketNumber = @ticketNumber", connection);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime checkInTime = Convert.ToDateTime(reader["checkIn"]);
                        TimeSpan totalTime = DateTime.UtcNow - checkInTime;
                        return $"{totalTime.Hours} Hours and {totalTime.Minutes} Minutes";
                    }
                }
                return "try again";
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while getting time parked in the transaction DAO: {e}");
                TimeSpan methodDuration = DateTime.UtcNow - start;
                return "oopsy poopsy";
            }
        }

        public Transaction BuildTransaction(SqlDataReader reader)
        {
            Transaction placeholder = new Transaction();
            placeholder.TicketNumber = (int)reader["ticketNumber"];
            placeholder.AmountPaid = (double)reader["amountPaid"];
            placeholder.Owner_carId = (int)reader["owner_carsId"];
            placeholder.CheckIn = (DateTime)reader["checkIn"];
            placeholder.CheckOut = reader["checkOut"].GetType() == typeof(System.DBNull) ? null : (DateTime?)reader["checkOut"];
            placeholder.ValetIdDroppingOff = (int)reader["valetDroppingOff"];
            placeholder.ValetIdPickingUp = reader["valetPickingUp"].GetType() == typeof(System.DBNull) ? null : (int?)reader["valetPickingUp"];
            return placeholder;
        }
        public decimal GetTransactionDollarAmount(int ticketNumber)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    DateTime checkIn;
                    DateTime? checkOut;
                    TimeSpan difference;
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select checkIn,checkOut from transactions
                    where ticketNumber = @ticketNumber", connection);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        checkIn = (DateTime)reader["checkIn"];
                        checkOut = reader["checkOut"].GetType() == typeof(System.DBNull) ? null : (DateTime?)reader["checkOut"];
                        difference = DateTime.UtcNow - checkIn;
                        decimal amount = ((int)difference.TotalHours +1) * 5m;
                        return amount;
                    }
                    return 0;
                }
            }
            catch(Exception e)
            {
                throw new Exception("something went wrong");
            }
        }
    }
}
