using CRMSystem.Data;
using CRMSystem.Models.DTOs;
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
        private readonly ICurrentUserServices _currentUserServices;

        public UserServices(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            ICurrentUserServices currentUserServices)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _currentUserServices = currentUserServices;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            var organizationId = _currentUserServices.OrganizationId;
            var currentUserRoles = _currentUserServices.Roles;

            var users = await _userRepository.GetAllUsers(organizationId);

            var result = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (currentUserRoles.Contains("SalesManager") && !roles.Contains("SalesRep"))
                {
                    continue;
                }

                var userDto = user.Adapt<UserResponseDto>();

                userDto.Roles = roles.ToList();

                result.Add(userDto);
            }

            return result;
        }

        public async Task<UserResponseDto?> GetUserById(Guid userId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var user = await _userRepository.GetUserById(
                userId,
                organizationId);

            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = user.Adapt<UserResponseDto>();

            userDto.Roles = roles.ToList();

            return userDto;
        }

        public async Task<IEnumerable<UserResponseDto>> GetUserByRole(string role)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var users = await _userRepository.GetUsersByRole(
                role,
                organizationId);

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

        public async Task<bool> DeleteUser(Guid userId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            return await _userRepository.DeleteUser(
                userId,
                organizationId);
        }
    }
}