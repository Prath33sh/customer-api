using CustomerApi.Models;

namespace CustomerApi.Services;
public interface ICustomerService
{
    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="request">The customer creation request.</param>
    /// <returns>The created customer response.</returns>
    Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request);

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="request">The customer update request.</param>
    /// <returns>The updated customer response.</returns>
    Task<CustomerResponse> UpdateCustomerAsync(CustomerUpdateRequest request);

    /// <summary>
    /// Deletes a customer by their ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer to delete.</param>
    Task DeleteCustomerAsync(Guid customerId);

    /// <summary>
    /// Gets a customer by their ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve.</param>
    Task<CustomerResponse> GetCustomerByIdAsync(Guid customerId);

}