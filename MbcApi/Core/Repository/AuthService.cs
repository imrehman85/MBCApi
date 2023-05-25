using MbcApi.Core.Dtos;
using MbcApi.Core.Entities;
using MbcApi.Core.Interfaces;
using MbcApi.Core.OtherObjects;
using MbcApi.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MbcApi.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private JwtTokenHelper _jwtTokenHelper;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, JwtTokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<AuthServiceResponseDto> LoginUser(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Invalid Credencials"
                };
            }

            bool isPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordIsCorrect)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Invalid Credencials"
                };
            }

            var userRole = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString())
            };

            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _jwtTokenHelper.GenerateJwtToken(claims);

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = token
            };
        }

        public async Task<AuthServiceResponseDto> RegisterUser(RegisterDto registerDto)
        {
            var isUserExist = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isUserExist != null)
                return new AuthServiceResponseDto
                {
                    IsSucceed=false,
                    Message= "User is already exist!"
                };
            ApplicationUser NewUser = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var isUserCreated = await _userManager.CreateAsync(NewUser, registerDto.Password);

            if (!isUserCreated.Succeeded)
            {
                var errorMsg = "User Registration Failed!";
                foreach (var error in isUserCreated.Errors)
                {
                    errorMsg = errorMsg + " " + error;
                }
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = errorMsg
                };
            }
            //add Role for User
            await _userManager.AddToRoleAsync(NewUser, StaticUserRoles.USER);

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "User registered successfully!"
            };
        }

        public async Task<AuthServiceResponseDto> SeedsRoles()
        {
            bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);
            bool isOwnerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);

            if (!isUserRoleExist)
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            if (!isOwnerRoleExist)
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
            if (!isAdminRoleExist)
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            
            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "Process on Data is Completed!"
            };
        }

        public async Task<AuthServiceResponseDto> MakeAdmin(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);
            if (user is null)
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Invalid Username"
                };

            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "Now its Admin"
            };
        }

        public async Task<AuthServiceResponseDto> MakeOwner(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);
            if (user is null)
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Invalid Username"
                };
            await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);
            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "Now its OWNER"
            };
        }

        public async Task<AuthServiceResponseDto> MakeUser(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);
            if (user is null)
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Invalid Username"
                };
            await _userManager.AddToRoleAsync(user, StaticUserRoles.USER);
            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "Now its USER"
            };
        }
    }
}
