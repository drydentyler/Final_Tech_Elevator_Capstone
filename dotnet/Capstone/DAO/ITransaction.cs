using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ITransaction
    {
        public Transaction AddNewTransaction(Transaction transaction);
        public Transaction UpdateTransaction(Transaction transaction);
        public Transaction GetTransactionByTicketNumber(int ticketNumber);
        public List<Transaction> GetAllTransactions();
        public decimal GetTransactionDollarAmount(int ticketNumber);
        public Transaction AddANewTransaction(int valetId, int owner_carsId);
        public string GetTimeParked(int ticketNumber);
    }
}
