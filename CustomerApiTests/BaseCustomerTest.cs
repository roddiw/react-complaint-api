using CustomerApi.Controllers;
using CustomerApi.Entities;
using CustomerApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace CustomerApiTests
{
    public abstract class BaseCustomerTest : IDisposable
    {
        private string dbName;
        private CustomerDbContext dbContext;

        protected Mock<ISettings> mockSettings;
        protected CustomerController customerController;

        public BaseCustomerTest()
        {
            // give each test a unique DB name, so that xUnit can run the tests in parallel
            dbName = "Customer" + Guid.NewGuid().ToString().Replace("-", "");
            dbContext = GetDbContext();

            mockSettings = new Mock<ISettings>();

            var customerRepository = new EfCustomerRepository(dbContext, mockSettings.Object);
            customerController = new CustomerController(customerRepository);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        protected void AddCustomerToDb(Customer customer)
        {
            using (var dbContext = GetDbContext())
            {
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }
        }

        protected CustomerDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
               .UseInMemoryDatabase(dbName)
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
               .Options;

            return new CustomerDbContext(options);
        }

        protected Customer GetLegalCustomer(long customerId)
        {
            return new Customer(customerId, "firstName", "lastName", DateTime.Now);
        }
    }
}
