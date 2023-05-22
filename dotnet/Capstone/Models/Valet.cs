using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class Valet
    {
        public ParkingLot ParkingLot { get; set; }

        public Valet(ParkingLot parkingLot)
        {
            ParkingLot = parkingLot;
        }
        /*
        public bool ParkCar(Car car)
        {
            if (ParkingLot.IsSpotAvailable())
            {
                car.ArrivalTime = DateTime.Now;
                car.ParkingSpot = ParkingLot.GetAvailableSpot();
                car.IsParked = true;
                ParkingLot.Cars.Add(car);
                return true;
            }

            return false;
        }
        public bool RetrieveCar(Car car)
        {
            if (ParkingLot.Cars.Contains(car))
            {
                car.DepartureTime = DateTime.Now;
                car.IsParked = false;
                ParkingLot.Cars.Remove(car);
                return true;
            }
            return false;
        }
        */
    }
}
