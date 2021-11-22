using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteinBackEnd.Model;
using SteinDataAccess;

namespace SteinBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IDataContext _dataContext;

        public InventoryController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult<List<InventoryModel>> GetAll()
        {
            try
            {
                return _dataContext.GetInventories()
                    .Select(s => new InventoryModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Kernels = s.Kernels
                    })
                    .ToList();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<InventoryModel> Get(int id)
        {
            try
            {
                var data = _dataContext.GetInventory(id);

                return new InventoryModel
                {
                    Id = data.Id,
                    Name = data.Name,
                    Kernels = data.Kernels
                };
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
