using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Models
{
    public class ParkingLot
    {
        public List<Car> Cars { get; set; }
        public int Capacity { get; set; }
        public double HourlyRate { get; set; }

        public ParkingLot(int capacity, double hourlyRate)
        {
            // Creates the lot of cars
            Cars = new List<Car>();
            Capacity = capacity;
            HourlyRate = hourlyRate;
        }

        public bool IsSpotAvailable()
        {
            return Cars.Count < Capacity;
        }
        /*
        public int GetAvailableSpot()
        {
            if (IsSpotAvailable())
            {
                for (int i = 1; i < Capacity; i++)
                {
                    if (!Cars.Any(c => c.ParkingSpot == i))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        */
    }
}
