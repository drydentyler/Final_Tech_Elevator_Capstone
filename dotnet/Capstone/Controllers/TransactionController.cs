using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

public class TransactionDTO
{
    public int ValetId { get; set; }
    public int AssociationId { get; set; }
}

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _transactionDao;

        public TransactionController(ITransaction dao)
        {
            _transactionDao = dao;
        }

        //*****tested and confirmed working in swagger
        [HttpGet]
        public List<Transaction> GetAllTransactions()
        {
            return _transactionDao.GetAllTransactions();
        }

        //*****tested and confirmed working in swagger
        [HttpGet("{ticketNumber}")]
        public Transaction GetTransactionByTicketNumber(int ticketNumber)
        {
            return _transactionDao.GetTransactionByTicketNumber(ticketNumber);
        }
        //*****tested and confirmed working in swagger
        [HttpPost]
        public Transaction AddNewTransaction(TransactionDTO dto)
        {
            return _transactionDao.AddANewTransaction(dto.ValetId,dto.AssociationId);
        }
        //*****tested and confirmed working in swagger
        //added more to sql statement to allow this end point to update either amount paid, checkout time, or valet picking up the car
        [HttpPut("{ticketNumber}")]
        public Transaction UpdateAmountPaid(int ticketNumber, Transaction transaction)
        {
            return _transactionDao.UpdateTransaction(transaction);
        }
        
        //*****tested and confirmed working in swagger
        [HttpGet("{ticketNumber}/Amount")]
        public decimal GetTransactionDollarAmount(int ticketNumber)
        {
            return _transactionDao.GetTransactionDollarAmount(ticketNumber);
        }

        // This needs a query key/value pair to work
        [HttpGet("{ticket}/time")]
        public ActionResult<string> ReturnTimeParked(int ticket = 0)
        {
            string timeParked;
            if(ticket == 0)
            {
                return BadRequest();
            }
            timeParked = _transactionDao.GetTimeParked(ticket);
            if (timeParked == null)
            {
                return BadRequest(timeParked);
            }
            else
            {
                return Ok(timeParked);
            }
        }

    }
}
