using Moq;
using CustomerApi.Models;
using CustomerApi.Data;
using CustomerApi.Data.Entities;
using CustomerApi.Services;
using CustomerApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class CustomerServiceTest
{
    private readonly ICustomerService _customerService;
    private readonly Mock<ILogger<CustomerApi.Services.CustomerService>> _mockLogger;
    private readonly CustomerServiceDBContext _dbContext;

    public CustomerServiceTest()
    {
        _mockLogger = new Mock<ILogger<CustomerApi.Services.CustomerService>>();
        var options = new DbContextOptionsBuilder<CustomerServiceDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new CustomerServiceDBContext(options);
        _customerService = new CustomerApi.Services.CustomerService(_dbContext, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsValidCustomerResponse_WhenSuccessful()
    {
        // Arrange
        var activeCustomer = await _customerService.CreateCustomerAsync(
                            new CustomerRequest{FirstName = "John", MiddleName = "M", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890"});

        // Act
        var result = await _customerService.GetCustomerByIdAsync(activeCustomer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(activeCustomer.Id, result.Id);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("M", result.MiddleName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("john@example.com", result.Email);
        Assert.Equal("1234567890", result.PhoneNumber);
    }

    [Fact]
    public async Task CreateCustomer_ThrowsException_WhenCustomerAlreadyExists()
    {
        // Arrange
        var activeCustomer = await AddTestCustomerAsync("Jake", "Doe", "jake@example.com", "1234567890");

        // Act & Assert
        await Assert.ThrowsAsync<CustomerAlreadyExistsException>(() => 
                        _customerService.CreateCustomerAsync(
                            new CustomerRequest {FirstName = "Jake", LastName = "Doe", Email = "jake@example.com", PhoneNumber = "1234567890"}));
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsCustomerResponse_WhenCustomerExistsAndNotDeleted()
    {
        // Arrange
        var activeCustomer = await AddTestCustomerAsync("John", "Doe", "john2@example.com", "1234567890");

        // Act
        var result = await _customerService.GetCustomerByIdAsync(activeCustomer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(activeCustomer.Id, result.Id);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ThrowsException_WhenCustomerDeleted()
    {
        // Arrange
        var deletedCustomer = await AddTestCustomerAsync("Jane", "Doe", "a@b.com", "0987654321", isDeleted: true);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _customerService.GetCustomerByIdAsync(deletedCustomer.Id));
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ThrowsException_WhenCustomerNotFound()
    {
        // Arrange
        // No customer is added to the context, so it should not find any customer.

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _customerService.GetCustomerByIdAsync(Guid.NewGuid()));
    }

    [Fact]
public async Task UpdateCustomerAsync_UpdatesCustomer_WhenCustomerExists()
{
    // Arrange
    var customer = await AddTestCustomerAsync("Alice", "Smith", "alice@example.com", "555-0000", middleName: "B");
    var updateRequest = new CustomerUpdateRequest
    {
        Id = customer.Id,
        FirstName = "Alicia",
        MiddleName = "Bee",
        LastName = "Smithers",
        Email = "alicia@example.com",
        PhoneNumber = "555-1111"
    };

    // Act
    var updated = await _customerService.UpdateCustomerAsync(updateRequest);

    // Assert
    Assert.NotNull(updated);
    Assert.Equal(updateRequest.Id, updated.Id);
    Assert.Equal("Alicia", updated.FirstName);
    Assert.Equal("Bee", updated.MiddleName);
    Assert.Equal("Smithers", updated.LastName);
    Assert.Equal("alicia@example.com", updated.Email);
    Assert.Equal("555-1111", updated.PhoneNumber);
}

[Fact]
public async Task UpdateCustomerAsync_ThrowsException_WhenCustomerNotFound()
{
    // Arrange
    var updateRequest = new CustomerUpdateRequest
    {
        Id = Guid.NewGuid(),
        FirstName = "Ghost",
        LastName = "Customer",
        Email = "ghost@example.com",
        PhoneNumber = "000-0000"
    };

    // Act & Assert
    await Assert.ThrowsAsync<NotFoundException>(() => _customerService.UpdateCustomerAsync(updateRequest));
}

[Fact]
public async Task DeleteCustomerAsync_SoftDeletesCustomer_WhenCustomerExists()
{
    // Arrange
    var customer = await AddTestCustomerAsync("Bob", "Brown", "bob@example.com", "555-2222");
    // Act
    await _customerService.DeleteCustomerAsync(customer.Id);

    // Assert
    var deleted = await _dbContext.Customers.FindAsync(customer.Id);
    Assert.NotNull(deleted);
    Assert.True(deleted.IsDeleted);
}

[Fact]
public async Task DeleteCustomerAsync_ThrowsException_WhenCustomerNotFound()
{
    // Arrange
    var id = Guid.NewGuid();

    // Act & Assert
    await Assert.ThrowsAsync<NotFoundException>(() => _customerService.DeleteCustomerAsync(id));
}

    private async Task<Customer> AddTestCustomerAsync(string firstName, string lastName, string email, string phoneNumber, bool isDeleted = false, string? middleName = null)
    {
        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            IsDeleted = isDeleted
        };
        if (middleName != null)
        {
            customer.MiddleName = middleName;
        }
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
        return customer;
    }
}