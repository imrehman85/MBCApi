using MbcApi.Core.Dtos;
using MbcApi.Core.Entities;
using MbcApi.Core.Interfaces;
using MbcApi.Core.OtherObjects;
using MbcApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace MbcApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iAuthService;

        public AuthController(IAuthService iAuthService)
        {
            _iAuthService = iAuthService;
        }

        [HttpPost]
        [Route("SeedsRoles")]
        public async Task<IActionResult> SeedsRole()
        {
            var result =await _iAuthService.SeedsRoles();

            return Ok(result);
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            var result = await _iAuthService.RegisterUser(registerDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _iAuthService.LoginUser(loginDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(result);
            }
        }

        [HttpPost]
        [Route("MakeAdmin")]
        [Authorize(Roles = StaticUserRoles.OWNER)]
        public async Task<IActionResult> MakeAdmin(UpdatePermissionDto updatePermissionDto)
        {
            var result = await _iAuthService.MakeAdmin(updatePermissionDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("MakeOwner")]
        [Authorize(Roles = StaticUserRoles.OWNER)]
        public async Task<IActionResult> MakeOwner(UpdatePermissionDto updatePermissionDto)
        {
            var result = await _iAuthService.MakeOwner(updatePermissionDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("MakeUser")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> MakeUser(UpdatePermissionDto updatePermissionDto)
        {
            var result = await _iAuthService.MakeUser(updatePermissionDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
