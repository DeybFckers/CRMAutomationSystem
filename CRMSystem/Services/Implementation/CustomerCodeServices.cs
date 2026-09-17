using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;

namespace CRMSystem.Services.Implementation
{
    public class CustomerCodeServices : ICustomerCodeServices
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCodeServices(
            ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<string> GenerateCustomerCode(
            Guid organizationId,
            string organizationName,
            DateTime date)
        {
            var prefix = GetCompanyInitial(organizationName);

            var customerCount =
                await _customerRepository.GetCustomerCountByMonth(
                    organizationId,
                    date.Year,
                    date.Month);

            var sequence = customerCount + 1;

            return $"{prefix}C{date.Year}{date.Month:D2}{sequence:D3}";
        }

        private string GetCompanyInitial(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return "X";

            var words = companyName
                .Trim()
                .Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);

            return words[0][0]
                .ToString()
                .ToUpper();
        }
    }
}
