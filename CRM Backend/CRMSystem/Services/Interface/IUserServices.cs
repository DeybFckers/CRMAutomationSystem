using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IUserServices
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsers();
        Task<UserResponseDto?> GetUserById(Guid userId);
        Task<IEnumerable<UserResponseDto>> GetUserByRole(string role);
        Task<bool> DeleteUser(Guid userId);
    }
}