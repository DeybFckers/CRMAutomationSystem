using CRMSystem.Models.DTOs;
using CRMSystem.Models.Responses;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class LeadConversionServices : ILeadConversionServices
    {
        private readonly ILeadConversionRepository _leadConversionRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public LeadConversionServices(ILeadConversionRepository leadConversionRepository, ICurrentUserServices currentUserServices)
        {
            _leadConversionRepository = leadConversionRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<PaginatedResponse<LeadConversionResponseDto>> GetAllLeadConversions(int page, int pageSize)
        {
            var organizationId = _currentUserServices.OrganizationId;

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result = await _leadConversionRepository.GetAllLeadConversions(
                organizationId,
                page,
                pageSize);

            var totalPages = result.TotalCount == 0
                ? 0
                : (int)Math.Ceiling((double)result.TotalCount / pageSize);

            return new PaginatedResponse<LeadConversionResponseDto>
            {
                Items = result.LeadConversions.Adapt<IEnumerable<LeadConversionResponseDto>>(),
                Pagination = new PaginationMetadata
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = result.TotalCount,
                    TotalPages = totalPages,
                    HasPreviousPage = page > 1,
                    HasNextPage = page < totalPages
                }
            };
        }

        public async Task<LeadConversionResponseDto> GetLeadConversionById(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leadConversion = await _leadConversionRepository.GetLeadConversionById(id, organizationId);

            if (leadConversion == null)
                throw new KeyNotFoundException("Lead conversion not found.");

            return leadConversion.Adapt<LeadConversionResponseDto>();
        }

        public async Task<LeadConversionResponseDto> UpdateLeadConversion(Guid id, UpdateLeadConversionDto leadConversion)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var existingLeadConversion = await _leadConversionRepository.GetLeadConversionById(id, organizationId);

            if (existingLeadConversion == null)
                throw new KeyNotFoundException("Lead conversion not found.");

            existingLeadConversion.ConvertedByUserId = leadConversion.ConvertedByUserId;
            existingLeadConversion.ConvertedAt = leadConversion.ConvertedAt;

            await _leadConversionRepository.UpdateLeadConversion(existingLeadConversion);

            return existingLeadConversion.Adapt<LeadConversionResponseDto>();
        }
    }
}