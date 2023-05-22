using System;
using System.Collections.Generic;
using System.Linq;
namespace Capstone.Models
{
    public class Car
    {
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string Color { get; set; }
        public bool needsPickedUp { get; set; }

        public Car(string licensePlate)
        {
            LicensePlate = licensePlate;
        }

        public Car()
        {

        }
    }
}
