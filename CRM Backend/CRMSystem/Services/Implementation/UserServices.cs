using CRMSystem.Data;
using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Services.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserServices(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers( Guid organizationId)
        {
            var organization = new Organization
            {
                Id = organizationId
            };

            var users = await _userRepository.GetAllUsers(organization);

            var result = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userDto = user.Adapt<UserResponseDto>();

                userDto.Roles = roles.ToList();

                result.Add(userDto);
            }

            return result;
        }

        public async Task<UserResponseDto?> GetUserById( Guid id, Guid organizationId)
        {
            var organization = new Organization
            {
                Id = organizationId
            };

            var user = await _userRepository.GetUserById(
                id,
                organization);

            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = user.Adapt<UserResponseDto>();

            userDto.Roles = roles.ToList();

            return userDto;
        }

        public async Task<IEnumerable<UserResponseDto>> GetUserByRole( string role, Guid organizationId)
        {
            var organization = new Organization
            {
                Id = organizationId
            };

            var users = await _userRepository.GetUsersByRole(
                role,
                organization);

            var result = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userDto = user.Adapt<UserResponseDto>();

                userDto.Roles = roles.ToList();

                result.Add(userDto);
            }

            return result;
        }

        public async Task<bool> DeleteUser( Guid id, Guid organizationId)
        {
            return await _userRepository.DeleteUser( id, organizationId);
        }
    }
}