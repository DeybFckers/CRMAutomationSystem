using CRMSystem.Models.Entities;
using CRMSystem.Services.Interface;

namespace CRMSystem.Services.Implementation
{
    public class AutomationServices : IAutomationServices
    {
        private readonly HttpClient _httpClient;

        public AutomationServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OrganizationCreated(Organization organization)
        {
            var payload = new
            {
                @event = "organization.created",
                organization = new
                {
                    organization.Id,
                    organization.Name,
                    organization.Email,
                    organization.Phone,
                    organization.Address
                }
            };

            await _httpClient.PostAsJsonAsync(
                "http://localhost:5678/webhook/organization-created",
                payload);
        }
    }
}
