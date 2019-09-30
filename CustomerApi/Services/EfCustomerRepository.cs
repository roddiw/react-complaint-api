using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Services
{
    public class EfCustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext dbContext;
        private readonly ISettings settings;

        public EfCustomerRepository(
            CustomerDbContext customerDbContext,
            ISettings settings)
        {
            this.dbContext = customerDbContext;
            this.settings = settings;
        }

        public async Task<long> AddCustomerAsync(Customer customer)
        {
            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();
            return customer.Id;
        }

        public async Task<bool> DeleteCustomerAsync(long customerId)
        {
            var customer = new Customer { Id = customerId };

            try
            {
                dbContext.Customers.Remove(customer);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<Customer> GetCustomerAsync(long customerId)
        {
            return await dbContext.Customers
                .Where(customer => customer.Id == customerId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Customer>> SearchCustomersAsync(
            string firstName, 
            string lastName)
        {
            firstName = (firstName ?? "").Trim().ToLower();
            lastName = (lastName ?? "").Trim().ToLower();

            return await dbContext.Customers
                .Where(customer =>
                    (string.IsNullOrEmpty(firstName) || customer.FirstName.ToLower().StartsWith(firstName))
                    && (string.IsNullOrEmpty(lastName) || customer.LastName.ToLower().StartsWith(lastName)))
                .Take(settings.SearchCustomers_MaxResults)
                .ToListAsync();
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                dbContext.Update<Customer>(customer);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
    }
}
