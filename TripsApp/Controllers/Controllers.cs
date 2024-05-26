using Microsoft.AspNetCore.Mvc;
using TripsApp.Models;
using TripsApp.Services;

namespace TripsApp.Controllers
{
        [ApiController]
        [Route("api/trips")]
        public class TripsController(ITripService tripService) : ControllerBase
        {
                [HttpGet]
                public async Task<IActionResult> GetAllTrips()
                {
                        var trips = await tripService.GetTrips();
                        return Ok(trips);
                }

                [HttpGet]
                public async Task<IActionResult> GetPaginatedTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
                {
                        if (page <= 0 || pageSize <= 0)
                        { 
                                return BadRequest("Page and PageSize must be greater than zero.");
                        }

                        var trips = await tripService.GetTrips(page, pageSize);
                        return Ok(trips);
                }
                
                [HttpPost("{idTrip}/clients")]
                public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientRequest request)
                {
                        if (await tripService.AssignClientToTrip(idTrip, request.Pesel, request.PaymentDate))
                        {
                                return Ok();
                        }

                        return BadRequest("Unable to assign client to trip.");
                }
        }

        [ApiController]
        [Route("api/clients")]
        public class ClientsController(IClientService clientService) : ControllerBase
        {
                [HttpDelete("{idClient}")]
                public async Task<IActionResult> DeleteClient(int idClient)
                {
                        var client = await clientService.DeleteClient(idClient);

                        if (client == null)
                        { 
                                return BadRequest("Client cannot be deleted because they have trips assigned.");
                        }

                        return Ok(client);
                }
        }
}