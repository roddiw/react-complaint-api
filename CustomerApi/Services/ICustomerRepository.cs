using CustomerApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public interface ICustomerRepository
    {
        Task<long> AddCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(long customerId);
        Task<Customer> GetCustomerAsync(long customerId);
        Task<List<Customer>> SearchCustomersAsync(string firstName, string lastName);
        Task<bool> UpdateCustomerAsync(Customer customer);
    }
}
