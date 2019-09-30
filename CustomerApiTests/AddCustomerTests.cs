using CustomerApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CustomerApiTests
{
    public class AddCustomerTests : BaseCustomerTest
    {
        [Fact]
        public async void AddCustomer_ReturnsCreatedAndCustomer_WhenFieldsValid()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 0);

            // act
            IActionResult result = await customerController.AddCustomerAsync(customer);

            // assert
            var objectResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedCustomer = Assert.IsType<Customer>(objectResult.Value);
            Assert.NotNull(returnedCustomer);
            Assert.Equal(returnedCustomer.Id, customer.Id);
            Assert.Equal(returnedCustomer.FirstName, customer.FirstName);
            Assert.Equal(returnedCustomer.LastName, customer.LastName);
            Assert.Equal(returnedCustomer.EmailAddress, customer.EmailAddress);
        }

        [Fact]
        public async void AddCustomer_ReturnsBadRequest_WhenFieldsInvalid()
        {
            // arrange
            var customer = GetLegalCustomer(customerId: 0);
            customer.FirstName = ""; // make invalid
            customerController.ModelState.AddModelError("FirstName", "Required");

            // act
            IActionResult result = await customerController.AddCustomerAsync(customer);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
