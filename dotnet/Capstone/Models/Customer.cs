using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class Customer
    {
        //public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public Customer()
        {

        }

        public Customer(/*int userId,*/ string firstName, string lastName, string phoneNumber)
        {
            //UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }
}
