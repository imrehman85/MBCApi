using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MbcApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> AddRooms()
        {
            return Ok();
        }
    }
}
