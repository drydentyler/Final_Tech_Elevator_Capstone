using Capstone.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IParkingLot
    {
        public bool AddCarToParkingLot(int parkingSpot, int ticketNumber);
        public void RemoveCarFromParkingLot(string licensePlate, int valetId);
        public bool CarIsCurrentlyParkedInLot(int ticketNumber);
        public List<Car> GetListOfCarsCurrentlyInTheLot();
        public int NumberOfSpotsAvailableInLot();
        public bool ToggleCarPickUpStatus(int ticketNumber);
        public List<int> ListOfAvailableSpots();
        public int GetParkingSpotByLicensePlate(string licesnsePlate);
        public SeeDetailsDTO SeeSpotDetails(int parkingSpotNumber);
    }
}
