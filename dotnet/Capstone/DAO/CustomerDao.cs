using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class CustomerDao : ICustomerDao
    {
		private readonly string connectionString;

		public CustomerDao(string dbConnectionString)
		{
			connectionString = dbConnectionString;
		}
        public Customer GetCustomerByTicketNumber(int ticketNumber)
        {
            try
            {
                Customer customer = new Customer();
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select firstName,lastName,phoneNumber from customers as c 
                      join owners_cars as oc on oc.customerId = c.customerId
                      join transactions as t on t.owner_carsId = oc.id
                      where ticketNumber = @ticketNumber", connection);
                    cmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        customer = BuildACustomer(reader);
                    }
                }
                return customer;
            }
            catch(Exception e)
            {
                throw new Exception("something went wrong getting customer by ticket number: " + e);
            }
        }
        public List<Customer> GetListOfCustomers()
        {
            try
            {
                List<Customer> listOfCurrentCustomers = new List<Customer>();
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select * from customers", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Customer placeholder = new Customer();
                        placeholder = BuildACustomer(reader);
                        listOfCurrentCustomers.Add(placeholder);
                    }
                }
                return listOfCurrentCustomers;
            }
            catch(Exception e)
            {
                throw new Exception("something went wrong");
            }
        }
        public int CreateNewCustomer(Customer customer)
        {
            try
            {
                Customer newCustomer = new Customer();
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"insert into customers(firstName,lastName,phoneNumber)
                    output inserted.customerId
                    values(@firstName,@lastName,@phoneNumber);", connection);
                    cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
                    int newCustomerId = (int)cmd.ExecuteScalar();
                    return newCustomerId;
                }
                
            }
            catch(Exception e)
            {
                throw new Exception("something went horribly wrong");
            }
        }
        public int? GetCustomerByDemoInfo(Customer customer)
        {
            try
            {
                Customer placeholder = new Customer();
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select customerId from customers
                    where firstName = @firstName and lastName = @lastName and phoneNumber = @phoneNumber", connection);
                    cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
                    cmd.Parameters.AddWithValue(@"lastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
                    return (int?)cmd.ExecuteScalar();
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception("database is probably on fire");
            }
        }
        public Customer BuildACustomer(SqlDataReader reader)
        {
            Customer placeholder = new Customer();
            placeholder.FirstName = (string)reader["firstName"];
            placeholder.LastName = (string)reader["lastName"];
            placeholder.PhoneNumber = (string)reader["phoneNumber"];
            return placeholder;
        }
        public Customer GetCustomerById(int id)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"select * from customers
                    where customerId = @id", connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return BuildACustomer(reader);
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception("something went wrong");
            }
        }
        public bool IsCurrentlyInDatabase(Customer customer)
        {
            return true;
        }
    }
}
