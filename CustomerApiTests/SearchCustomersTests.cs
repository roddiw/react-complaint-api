using CustomerApi.Controllers;
using CustomerApi.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CustomerApiTests
{
    public class SearchCustomersTests : BaseCustomerTest
    {
        public SearchCustomersTests()
        {
            mockSettings.Setup(mock => mock.SearchCustomers_MaxResults).Returns(10);
        }

        [Fact]
        public async void SearchCustomers_ReturnsOkAndCustomers_WhenNameMatches()
        {
            // arrange
            string lastName = "Smith";
            AddCustomerToDb(new Customer(0, "Fred", lastName, DateTime.Now));
            AddCustomerToDb(new Customer(0, "Frodo", "Baggins", DateTime.Now));
            AddCustomerToDb(new Customer(0, "Jane", "Bloggs", DateTime.Now));

            // act
            IActionResult result = await customerController.SearchCustomersAsync("", lastName.Substring(0, 2));

            // assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(objectResult.Value);
            Assert.NotNull(returnedCustomers);
            Assert.Single(returnedCustomers);
            Assert.Equal(lastName, returnedCustomers.First().LastName);
        }

        [Fact]
        public async void SearchCustomers_ReturnsOkAndNoCustomers_WhenNameDoesntMatch()
        {
            // arrange
            string nonMatchingFirstName = "Xxx";
            string matchingLastName = "Smith";
            AddCustomerToDb(new Customer(0, "Fred", matchingLastName, DateTime.Now));
            AddCustomerToDb(new Customer(0, "Frodo", "Baggins", DateTime.Now));
            AddCustomerToDb(new Customer(0, "Jane", "Bloggs", DateTime.Now));

            // act
            IActionResult result = await customerController.SearchCustomersAsync(nonMatchingFirstName.Substring(0, 2), matchingLastName.Substring(0, 2));

            // assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(objectResult.Value);
            Assert.NotNull(returnedCustomers);
            Assert.Empty(returnedCustomers);
        }

        [Fact]
        public async void SearchCustomers_ReturnsBadRequestAndErrorDetails_WhenFirstNameAndLastNameAreEmpty()
        {
            // arrange
            string firstName = "";
            string lastName = "";

            // act
            IActionResult result = await customerController.SearchCustomersAsync(firstName, lastName);

            // assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<ErrorDetails>(objectResult.Value);
        }
    }
}
