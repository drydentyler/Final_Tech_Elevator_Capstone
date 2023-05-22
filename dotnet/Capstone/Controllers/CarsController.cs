using Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarSqlDao carSqlDao;

        public CarsController(ICarSqlDao _carSqlDao)
        {
            carSqlDao = _carSqlDao;
        }

        [HttpGet]
        public ActionResult<List<Car>> GetAllCars()
        {
            List<Car> listToReturn = carSqlDao.ReturnAllCarsList();

            if ((listToReturn.Count == 0) || (listToReturn == null))
            {
                return NotFound(listToReturn);
            }
            else
            {
                return Ok(listToReturn);
            }
        }

        [HttpGet("{plate}")]
        public ActionResult<Car> GetCarByPlate(string plate)
        {
            Car carToReturn = carSqlDao.GetCar(plate);

            if (carToReturn == null)
            {
                return NotFound(carToReturn);
            }
            else
            {
                return Ok(carToReturn);
            }
        }
        //!!!!!!!NEEDS SQL STATEMENT WRITTEN
        [HttpGet("/pickup")]
        public ActionResult<List<Car>> GetCarToPickUp()
        {
            List<Car> carToReturn = carSqlDao.CarToPickUp();

            if (carToReturn == null)
            {
                return NotFound(carToReturn);
            }
            else
            {
                return Ok(carToReturn);
            }
        }

        [HttpPut("/pickup")]
        public ActionResult<bool> PostCarNeedsPickedUp(int ticket = 0)
        {
            if (ticket == 0)
            {
                return BadRequest(false);
            }
            bool carAddedToPickUp = carSqlDao.AddCarToPickUp(ticket);
            if (carAddedToPickUp == false)
            {
                return BadRequest(carAddedToPickUp);
            }
            else
            {
                return Ok(carAddedToPickUp);
            }
        }
        //*****tested and confirmed working in swagger
        [HttpPost]
        public Car AddNewCarRecord(Car car)
        {
            ActionResult<Car> result = carSqlDao.GetCar(car.LicensePlate);
            if(result.Value == null)
            {
                return carSqlDao.AddNewCar(car);
            }
            else
            {
                return result.Value;
            }
        }
    }
}
