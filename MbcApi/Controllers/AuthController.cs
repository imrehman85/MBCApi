using MbcApi.Core.Dtos;
using MbcApi.Core.OtherObjects;
using MbcApi.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MbcApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private JwtTokenHelper _jwtTokenHelper;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, JwtTokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost]
        [Route("SeedsRoles")]
        public async Task<IActionResult> SeedsRole()
        {
            bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);
            bool isOwnerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);

            if(!isUserRoleExist)
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            if (!isOwnerRoleExist)
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
            if (!isAdminRoleExist)
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));

            return Ok("Process on Data is Completed!");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            var isUserExist = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isUserExist != null)
                return BadRequest("User is already exist!");
            IdentityUser NewUser = new IdentityUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                SecurityStamp=Guid.NewGuid().ToString()
            };

            var isUserCreated = await _userManager.CreateAsync(NewUser, registerDto.Password);

            if (!isUserCreated.Succeeded)
            {
                var errorMsg = "User Registration Failed!";
                foreach(var error in isUserCreated.Errors)
                {
                    errorMsg = errorMsg + " " + error;
                }
                return BadRequest(errorMsg);
            }
            //add Role for User
            await _userManager.AddToRoleAsync(NewUser, StaticUserRoles.USER);
             
            return Ok("User registered successfully!");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user =await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null)
            {
                return Unauthorized("Invalid Credencials");
            }

            bool isPasswordIsCorrect =await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordIsCorrect)
            {
                return Unauthorized("Invalid Credencials");
            }

            var userRole=await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString())
            };

            foreach(var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _jwtTokenHelper.GenerateJwtToken(claims);
            return Ok(token);
        }

        [HttpPost]
        [Route("MakeAdmin")]
        public async Task<IActionResult> MakeAdmin(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);
            if (user is null)
                return BadRequest("Invalid Username");
            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);
            return Ok("Now its Admin");
        }

        [HttpPost]
        [Route("MakeOwner")]
        public async Task<IActionResult> MakeOwner(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);
            if (user is null)
                return BadRequest("Invalid Username");
            await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);
            return Ok("Now its Owner");
        }

        [HttpPost]
        [Route("MakeUser")]
        public async Task<IActionResult> MakeUser(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);
            if (user is null)
                return BadRequest("Invalid Username");
            await _userManager.AddToRoleAsync(user, StaticUserRoles.USER);
            return Ok("Now its User");
        }
    }
}
