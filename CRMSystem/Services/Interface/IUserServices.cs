using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IUserServices
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsers( Guid organizationId);

        Task<UserResponseDto?> GetUserById( Guid id, Guid organizationId);

        Task<IEnumerable<UserResponseDto>> GetUserByRole( string role, Guid organizationId);
        Task<bool> DeleteUser(Guid id, Guid organizationId);
    }
}