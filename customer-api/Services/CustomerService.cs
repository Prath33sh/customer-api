using CustomerApi.Models;
using CustomerApi.Data;
using CustomerApi.Data.Entities;
using CustomerApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Services;

public class CustomerService(CustomerServiceDBContext dbContext, ILogger<CustomerService> logger) : ICustomerService
{
    public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request)
    {
        if (await dbContext.Customers.AnyAsync(u =>
                                                u.Email != null && request.Email != null &&
                                                u.Email.ToLower() == request.Email.ToLower()))
        {
            throw new CustomerAlreadyExistsException("Customer already exists with the same Email");
        }
        var customer = new Customer()
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber

        };

        try
        {
            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating customer");
            throw new DBOperationException("Error creating customer");
        }

        return new CustomerResponse
        {
            Id = customer.Id, 
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }

    public async Task<CustomerResponse> UpdateCustomerAsync(CustomerUpdateRequest request)
    {
        var customer = await dbContext.Customers.Where(u => u.Id == request.Id && !u.IsDeleted).SingleOrDefaultAsync();
        if (customer == null)
        {
            logger.LogWarning("Customer with ID {CustomerId} not found for update", request.Id);
            throw new NotFoundException("Customer not found");
        }
        customer.FirstName = request.FirstName;
        customer.MiddleName = request.MiddleName; 
        customer.LastName = request.LastName;
        customer.Email = request.Email; 
        customer.PhoneNumber = request.PhoneNumber;
        try
        {
            dbContext.Customers.Update(customer);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating customer with ID {CustomerId}", request.Id);
            throw new DBOperationException("Error updating customer");
        }

        return new CustomerResponse
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }
    public async Task DeleteCustomerAsync(Guid customerId)
    {
        var customer = await dbContext.Customers.Where(u => u.Id == customerId && !u.IsDeleted).SingleOrDefaultAsync();
        if (customer == null)
        {
            logger.LogWarning("Customer with ID {CustomerId} not found for deletion", customerId);
            throw new NotFoundException("Customer not found");
        }
        customer.IsDeleted = true; // Soft delete
        try
        {
            dbContext.Customers.Update(customer);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting customer with ID {CustomerId}", customerId);
            throw new DBOperationException("Error deleting customer");
        }
    }

    public async Task<CustomerResponse> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await dbContext.Customers.Where(u => u.Id == customerId && !u.IsDeleted).SingleOrDefaultAsync();
        if (customer == null)
        {
            logger.LogWarning("Customer with ID {CustomerId} not found", customerId);
            throw new NotFoundException("Customer not found");
        }

        return new CustomerResponse
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }
}