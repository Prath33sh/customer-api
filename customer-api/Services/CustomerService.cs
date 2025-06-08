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
        if (await dbContext.Customers.AnyAsync(u => u.Email == request.Email))
        {
            throw new CustomerAlreadyExistsException("Customer already exists with the same Email");
        }
        var user = new Customer()
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber

        };

        try
        {
            await dbContext.Customers.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user");
            throw new DBOperationException("Error creating user");
        }

        return new CustomerResponse
        {
            Id = user.Id, 
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<CustomerResponse> UpdateCustomerAsync(CustomerUpdateRequest request)
    {
        var user = await dbContext.Customers.Where(u => u.Id == request.Id && !u.IsDeleted).SingleOrDefaultAsync();
        if (user == null)
        {
            logger.LogWarning("Customer with ID {CustomerId} not found for update", request.Id);
            throw new NotFoundException("Customer not found");
        }
        user.FirstName = request.FirstName;
        user.MiddleName = request.MiddleName; 
        user.LastName = request.LastName;
        user.Email = request.Email; 
        user.PhoneNumber = request.PhoneNumber;
        try
        {
            dbContext.Customers.Update(user);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user with ID {CustomerId}", request.Id);
            throw new DBOperationException("Error updating user");
        }

        return new CustomerResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }
    public async Task DeleteCustomerAsync(Guid userId)
    {
        var user = await dbContext.Customers.Where(u => u.Id == userId && !u.IsDeleted).SingleOrDefaultAsync();
        if (user == null)
        {
            logger.LogWarning("Customer with ID {CustomerId} not found for deletion", userId);
            throw new NotFoundException("Customer not found");
        }
        user.IsDeleted = true; // Soft delete
        try
        {
            dbContext.Customers.Update(user);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting user with ID {CustomerId}", userId);
            throw new DBOperationException("Error deleting user");
        }
    }

    public async Task<CustomerResponse> GetCustomerByIdAsync(Guid userId)
    {
        var user = await dbContext.Customers.Where(u => u.Id == userId && !u.IsDeleted).SingleOrDefaultAsync();
        if (user == null)
        {
            logger.LogWarning("Customer with ID {CustomerId} not found", userId);
            throw new NotFoundException("Customer not found");
        }

        return new CustomerResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }
}