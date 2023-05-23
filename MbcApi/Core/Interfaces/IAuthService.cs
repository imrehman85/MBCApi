using MbcApi.Core.Dtos;

namespace MbcApi.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedsRoles();
        Task<AuthServiceResponseDto> RegisterUser(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginUser(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeAdmin(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeOwner(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeUser(UpdatePermissionDto updatePermissionDto);
    }
}
