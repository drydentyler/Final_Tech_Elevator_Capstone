using System.Collections.Generic;

namespace Capstone.Models
{
    public interface ICarSqlDao
    {
        List<Car> ReturnAllCarsList();
        Car GetCar(string licensePlate);
        List<Car> CarToPickUp();
        bool AddCarToPickUp(int ticket);
        Car AddNewCar(Car car);

    }
}
