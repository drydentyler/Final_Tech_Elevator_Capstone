using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class ActiveLotDTO
{
    public int parkingSpotId { get; set; }
    public int ticketNumber { get; set; }
}

public class ActiveCarDTO
{
    public Car car { get; set; }
    public int parkingSpotId { get; set; }
}

public class SeeDetailsDTO
{
    public Customer customer { get; set; }
    public int ticketNumber { get; set; }
}

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingLotController : ControllerBase
    {
        private readonly IParkingLot _parkinglotDao;
        public ParkingLotController(IParkingLot dao)
        {
            _parkinglotDao = dao;
        }

        //*****tested and confirmed working in swagger
        //returns true if car is currently in the lot
        [HttpGet("{ticketNumber}")]
        public bool CheckIfCarIsCurrentlyInLot(int ticketNumber)
        {
            return _parkinglotDao.CarIsCurrentlyParkedInLot(ticketNumber);
        }

        //*****tested and confirmed working in swagger
        //returns list of cars with information: licenseplate, make, model, vin, and color

        //should perform a join to provide back user/customer information?
        [HttpGet]
        public List<ActiveCarDTO> GetListOfAllCarsCurrentlyInLot()
        {
            List<ActiveCarDTO> list = new List<ActiveCarDTO>();
            
            List<Car> listOfCars = _parkinglotDao.GetListOfCarsCurrentlyInTheLot();
            foreach(Car car in listOfCars)
            {
                ActiveCarDTO dto = new ActiveCarDTO();
                int parkingSpot = _parkinglotDao.GetParkingSpotByLicensePlate(car.LicensePlate);
                dto.car = car;
                dto.parkingSpotId = parkingSpot;
                list.Add(dto);
            }
            return list;
        }

        //need to check why in swagger it is showing 2 parameters, a string and an integer
        //the actual sql statement to perform the 'delete' is an update, is that alright or does it have to be a delete?

        //*****tested and confirmed working in swagger
        //returns false if the car has been removed, as in the car is no longer in the lot
        [HttpDelete]
        public void RemoveCarFromParkingLot(string licensePlate, int valetId)
        {
            _parkinglotDao.RemoveCarFromParkingLot(licensePlate, valetId);
        }

        //*****tested and confirmed working in swagger
        //used to add a car to the active parking lot
        [HttpPut("spot/{parkingSpot}")]
        public bool AddACarToTheParkingLot(ActiveLotDTO dto)
        {
            return _parkinglotDao.AddCarToParkingLot(dto.parkingSpotId, dto.ticketNumber);
        }

        //*****Tested and confirmed working in swagger
        //used to generate the number of available spots in the parking lot
        [HttpGet("/AvailableSpots")]
        public int NumberOfSpotsAvailable()
        {
            return _parkinglotDao.NumberOfSpotsAvailableInLot();
        }

        //*****tested and confirmed working in swagger
        [HttpPut("ticket/{ticketNumber}")]
        public bool ToggleCarPickUpStatusByTicketNumber(int ticketNumber)
        {
            return _parkinglotDao.ToggleCarPickUpStatus(ticketNumber);
        }
        [HttpGet("/AvailableSpots/list")]
        public List<int> ListOfAvailableSpots()
        {
            return _parkinglotDao.ListOfAvailableSpots();
        }
        [HttpGet("{parkingSpotNumber}/Details")]
        public SeeDetailsDTO GetParkingSpotDetails(int parkingSpotNumber)
        {
            return _parkinglotDao.SeeSpotDetails(parkingSpotNumber);
        }
    }
}
