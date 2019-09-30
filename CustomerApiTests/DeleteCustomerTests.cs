using CustomerApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CustomerApiTests
{
    public class DeleteCustomerTests : BaseCustomerTest
    {
        [Fact]
        public async void DeleteCustomer_ReturnsNoContent_WhenCustomerExists()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 0);
            AddCustomerToDb(customer);
            long savedCustomerId = customer.Id;

            // act
            IActionResult result = await customerController.DeleteCustomerAsync(savedCustomerId);

            // assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteCustomer_ReturnsNotFoundAndErrorDetails_WhenCustomerDoesntExist()
        {
            // arrange
            long customerId = 1;

            // act
            IActionResult result = await customerController.DeleteCustomerAsync(customerId);

            // assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<ErrorDetails>(objectResult.Value);
        }
    }
}
