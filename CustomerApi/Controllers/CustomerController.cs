using CustomerApi.Entities;
using CustomerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /Customer/1
        /// </remarks>
        /// <param name="customerId">The customer's ID</param>
        /// <returns>The customer</returns>
        /// <response code="200">If the customer was found</response>
        /// <response code="404">If the customer does not exist</response>
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerAsync(long customerId)
        {
            Customer customer = await customerRepository.GetCustomerAsync(customerId);
            if (customer == null)
            {
                return NotFound(GetCustomerNotFoundError(customerId));
            }

            return Ok(customer);
        }

        /// <summary>
        /// Gets customers with matching first and/or last names.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /Customer?firstName=John
        ///     
        ///     Sample request:
        ///     GET /Customer?lastName=Smith
        ///     
        ///     Sample request:
        ///     GET /Customer?firstName=Jo&amp;lastName=Sm
        /// </remarks>
        /// <param name="firstName">The first name, or starting fragment, to match</param>
        /// <param name="lastName">The last name, or starting fragment, to match</param>
        /// <returns>A list of matching customers</returns>
        /// <response code="200">If customers were searched</response>
        /// <response code="400">If there was a bad request parameter</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchCustomersAsync(
            string firstName,
            string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            {
                return BadRequest(new ErrorDetails("firstName and lastName cannot both be empty"));
            }

            IEnumerable<Customer> customers = await customerRepository.SearchCustomersAsync(firstName, lastName);
            return Ok(customers);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /Customer
        ///     {
        ///         "id": 0,
        ///         "firstName": "Jane",
        ///         "lastName": "Smith"
        ///     }
        /// </remarks>
        /// <param name="customer">The customer to create. customer.Id is ignored.</param>
        /// <returns>The created customer</returns>
        /// <response code="201">If the customer was successfully created</response>
        /// <response code="400">If there was a bad request parameter</response>
        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCustomerAsync(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long customerId = await customerRepository.AddCustomerAsync(customer);
            customer.Id = customerId;
            return CreatedAtAction(nameof(GetCustomerAsync), new { customerId }, customer);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     PUT /Customer
        ///     {
        ///         "id": 1,
        ///         "firstName": "Jane",
        ///         "lastName": "Smith"
        ///     }
        /// </remarks>
        /// <param name="customerId">The customer's ID</param>
        /// <param name="customer">The customer's details. customer.Id must be the same as customerId</param>
        /// <response code="204">If the customer was successfully updated</response>
        /// <response code="400">If there was a bad request parameter</response>
        /// <response code="404">If the customer does not exist</response>
        [HttpPut("{customerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomerAsync(
            long customerId,
            Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (customerId != customer.Id)
            {
                return BadRequest(new ErrorDetails("customerId and customer.id must be the same"));
            }

            customer.Id = customerId;
            bool isUpdated = await customerRepository.UpdateCustomerAsync(customer);
            if (!isUpdated)
            {
                return NotFound(GetCustomerNotFoundError(customerId));
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     DELETE /Customer/1
        /// </remarks>
        /// <param name="customerId">The customer's ID</param>
        /// <response code="204">If the customer was successfully deleted</response>
        /// <response code="404">If the customer does not exist</response>
        [HttpDelete("{customerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomerAsync(long customerId)
        {
            bool isDeleted = await customerRepository.DeleteCustomerAsync(customerId);
            if (!isDeleted)
            {
                return NotFound(GetCustomerNotFoundError(customerId));
            }

            return NoContent();
        }

        private ErrorDetails GetCustomerNotFoundError(long customerId)
        {
            return new ErrorDetails($"customer {customerId} not found");
        }
    }
}
