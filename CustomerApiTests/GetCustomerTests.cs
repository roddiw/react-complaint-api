using CustomerApi.Controllers;
using CustomerApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CustomerApiTests
{
    public class GetCustomerTests : BaseCustomerTest
    {
        [Fact]
        public async void GetCustomer_ReturnsOkAndCustomer_WhenCustomerExists()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 0);
            AddCustomerToDb(customer);
            long savedCustomerId = customer.Id;

            // act
            IActionResult result  = await customerController.GetCustomerAsync(customer.Id);

            // assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<Customer>(objectResult.Value);
            Assert.NotNull(returnedCustomer);
            Assert.Equal(returnedCustomer.Id, savedCustomerId);
            Assert.Equal(returnedCustomer.FirstName, customer.FirstName);
            Assert.Equal(returnedCustomer.LastName, customer.LastName);
            Assert.Equal(returnedCustomer.DateOfBirth, customer.DateOfBirth);
        }

        [Fact]
        public async void GetCustomer_ReturnsNotFoundAndErrorDetails_WhenCustomerDoesntExist()
        {
            // arrange
            long nonExistentCustomerId = 1;

            // act
            IActionResult result = await customerController.GetCustomerAsync(nonExistentCustomerId);

            // assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<ErrorDetails>(objectResult.Value);
        }
    }
}
