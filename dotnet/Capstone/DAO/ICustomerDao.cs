using Capstone.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICustomerDao
    {
        public int CreateNewCustomer(Customer customer);
        public int? GetCustomerByDemoInfo(Customer customer);
        public Customer GetCustomerById(int id);
        public List<Customer> GetListOfCustomers();
        public Customer GetCustomerByTicketNumber(int ticketNumber);
    }
}
