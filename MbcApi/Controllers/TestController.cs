using MbcApi.Core.OtherObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MbcApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        public TestController()
        {
        }

        [HttpGet]
        [Route("GetByAdmin")]
        [Authorize(Roles =StaticUserRoles.ADMIN)]
        public IActionResult Get1()
        {
            return Ok(Summaries);
        }

        [HttpGet]
        [Route("GetByUser")]
        [Authorize(Roles = StaticUserRoles.USER)]
        public IActionResult Get2()
        {
            return Ok(Summaries);
        }

        [HttpGet]
        [Route("GetByOwner")]
        [Authorize(Roles = StaticUserRoles.OWNER)]
        public IActionResult Get3()
        {
            return Ok(Summaries);
        }
    }
}