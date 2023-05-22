using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerDao _customerDao;

        public CustomersController(ICustomerDao customerDao)
        {
            _customerDao = customerDao;
        }
        /*
        [HttpGet]
        public Customer FindCustomerByDemoInfo(Customer customer)
        {
            int? customerId = _customerDao.GetCustomerByDemoInfo(customer);
            return customerId == null ? null : _customerDao.GetCustomerById((int)customerId);
        }*/

        //*****tested and confirmed working in swagger 
        [HttpPost]
        public int AddNewCustomer(Customer customer)
        {
            int? customerId = _customerDao.GetCustomerByDemoInfo(customer);
            return customerId == null ? _customerDao.CreateNewCustomer(customer) : (int)customerId;
        }
        [HttpGet("{ticketNumber}")]
        public Customer GetCustomerInformationByTicketNumber(int ticketNumber)
        {
           return _customerDao.GetCustomerByTicketNumber(ticketNumber);
        }
        //[HttpGet("List")]
        //public List<Customer> GetListOfAllCustomers()
        //{

        //}

    }
}
