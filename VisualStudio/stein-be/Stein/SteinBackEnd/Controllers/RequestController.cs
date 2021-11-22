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
    public class RequestController : ControllerBase
    {
        private readonly IDataContext _dataContext;

        public RequestController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult<List<RequestModel>> GetAll()
        {
            try
            {
                var inventory = _dataContext.GetInventories();
                var data = _dataContext.GetRequests()
                    .Select(s => new RequestModel
                    {
                        Id = s.Id,
                        InventoryId = s.InventoryId,
                        InventoryName = inventory.FirstOrDefault(f => f.Id == s.InventoryId)?.Name,
                        RequestedKernels = s.RequestedKernels
                    })
                    .ToList();
                    
                return data;
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<RequestModel> Get(int id)
        {
            try
            {
                var baseData = _dataContext.GetRequest(id);
                var inventory = _dataContext.GetInventory(baseData.InventoryId);

                return new RequestModel
                {
                    Id = baseData.Id,
                    InventoryId = baseData.InventoryId,
                    InventoryName = inventory?.Name,
                    RequestedKernels = baseData.RequestedKernels
                };
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpPost("{id:int}")]
        public ActionResult<RequestProcessResult> Process(int id, [FromBody]Request request)
        {
            try
            {
                if (request.Id != id)
                    return BadRequest();
                
                var inventory = _dataContext.GetInventory(request.InventoryId);

                if (request.RequestedKernels > inventory.Kernels)
                {
                    return new RequestProcessResult
                    {
                        Message = "There is no enough kernels in inventory.",
                        Result = null
                    };
                }

                inventory.Kernels = inventory.Kernels - request.RequestedKernels;
                _dataContext.UpdateInventory(inventory);

                inventory = _dataContext.GetInventory(request.InventoryId);

                var result = new RequestResult
                {
                    Id = request.Id,
                    InventoryId = request.InventoryId,
                    InventoryName = inventory?.Name,
                    Kernels = inventory?.Kernels ?? 0,
                    RequestedKernels = request.RequestedKernels
                };

                return new RequestProcessResult
                {
                    Message = "Request processed successfully.",
                    Result = result
                };
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
