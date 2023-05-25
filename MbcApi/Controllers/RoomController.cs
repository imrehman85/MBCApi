using MbcApi.Core.Entities;
using MbcApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MbcApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;

        public RoomController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = _unitOfWork.roomRepo.GetAll();

            return Ok(result);
        }

        [HttpPost]
        [Route("AddRooms")]
        public async Task<IActionResult> AddRooms()
        {
            Rooms rooms = new Rooms
            {
                Id=Guid.NewGuid().ToString(),
                RoomNumber="A2",
                Floor="First",
                TotalBed=1,
                TotalCapacity=4, 
                IsAttachBath =false,
                IsAttachKitchen =false,
                Status="Active"

            };
            _unitOfWork.roomRepo.Add(rooms);
            _unitOfWork.Complete();

            return Ok("Data Added");
        }
    }
}
