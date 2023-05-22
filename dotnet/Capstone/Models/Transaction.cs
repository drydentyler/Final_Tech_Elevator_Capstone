using System;

namespace Capstone.Models
{
    public class Transaction
    {
        public int TicketNumber { get; set; }
        public double AmountPaid { get; set; }
        public int Owner_carId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; } = null;
        public int ValetIdDroppingOff { get; set; }
        public int? ValetIdPickingUp { get; set; } = null;

        public Transaction()
        {

        }
        public Transaction(int ticketNumber, double amountPaid, int owner_carId, DateTime checkIn,
            DateTime? checkout, int valetDroppingOffId, int? valetPickingUpId)
        {
            TicketNumber = ticketNumber;
            AmountPaid = amountPaid;
            Owner_carId = owner_carId;
            CheckIn = checkIn;
            CheckOut = checkout;
            ValetIdDroppingOff = valetDroppingOffId;
            ValetIdPickingUp = valetPickingUpId;

        }
    }
}
