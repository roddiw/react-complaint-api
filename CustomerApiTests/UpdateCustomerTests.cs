using CustomerApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CustomerApiTests
{
    public class UpdateCustomerTests : BaseCustomerTest
    {
        [Fact]
        public async void UpdateCustomer_ReturnsNoContent_WhenCustomerExists()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 0);
            AddCustomerToDb(customer);

            // act
            IActionResult result = await customerController.UpdateCustomerAsync(customer.Id, customer);

            // assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdateCustomer_ReturnsNotFoundAndErrorDetails_WhenCustomerDoesntExist()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 1);

            // act
            IActionResult result = await customerController.UpdateCustomerAsync(customer.Id, customer);

            // assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<ErrorDetails>(objectResult.Value);
        }

        [Fact]
        public async void UpdateCustomer_ReturnsBadRequestAndErrorDetails_WhenCustomerIDsDontMatch()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 1);

            // act
            IActionResult result = await customerController.UpdateCustomerAsync(customer.Id+1, customer);

            // assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<ErrorDetails>(objectResult.Value);
        }

        [Fact]
        public async void UpdateCustomer_ReturnsBadRequest_WhenFieldsInvalid()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 0);
            AddCustomerToDb(customer);
            customer.FirstName = ""; // make invalid
            customerController.ModelState.AddModelError("FirstName", "Required");

            // act
            IActionResult result = await customerController.UpdateCustomerAsync(customer.Id, customer);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
