using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class Owner_DTO
{
    public int id { get; set; }
    public string licensePlate { get; set; }
}


namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   

    public class Owners_CarsController : ControllerBase
    {
        private readonly IOwners_Cars _ownersCarsDao;
        public Owners_CarsController(IOwners_Cars dao)
        {
            _ownersCarsDao = dao;
        }
        [HttpPost]
        public int AddNewCustomerCarAssociation(Owner_DTO dto)
        {
            int associationId = _ownersCarsDao.AddNewRecordOwnersCars(dto.id, dto.licensePlate);
            return associationId;
        }
    }
}
