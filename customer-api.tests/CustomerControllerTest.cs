using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CustomerApi.Controllers;
using CustomerApi.Models;
using CustomerApi.Services;
using CustomerApi.Exceptions;
using Microsoft.Extensions.Logging;
using Xunit;

public class CustomerControllerTest
{
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly Mock<ILogger<CustomerController>> _mockLogger;
    private readonly CustomerController _controller;

    public CustomerControllerTest()
    {
        _mockCustomerService = new Mock<ICustomerService>();
        _mockLogger = new Mock<ILogger<CustomerController>>();
        _controller = new CustomerController(_mockLogger.Object, _mockCustomerService.Object);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsCreatedAtAction_WhenSuccessful()
    {
        // Arrange
        var request = new CustomerRequest { FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
        var response = new CustomerResponse { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
        _mockCustomerService.Setup(s => s.CreateCustomerAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.CreateCustomer(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetCustomerById), createdResult.ActionName);
        Assert.Equal(response, createdResult.Value);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsOk_WhenCustomerExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = new CustomerResponse { Id = id, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "555-1234" };
        _mockCustomerService.Setup(s => s.GetCustomerByIdAsync(id)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetCustomerById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var request = new CustomerUpdateRequest { Id = Guid.NewGuid(), FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123" };
        var response = new CustomerResponse { Id = request.Id, FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123" };
        _mockCustomerService.Setup(s => s.UpdateCustomerAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.UpdateCustomer(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task DeleteCustomer_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockCustomerService.Setup(s => s.DeleteCustomerAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCustomer(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

        [Fact]
    public async Task CreateCustomer_Returns500_WhenDBOperationException()
    {
        // Arrange
        var request = new CustomerRequest { FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
        _mockCustomerService.Setup(s => s.CreateCustomerAsync(request)).ThrowsAsync(new DBOperationException("DB error"));

        // Act
        var result = await _controller.CreateCustomer(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetCustomerById_Returns500_WhenUnexpectedException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockCustomerService.Setup(s => s.GetCustomerByIdAsync(id)).ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await _controller.GetCustomerById(id);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
      }

    [Fact]
    public async Task UpdateCustomer_Returns500_WhenDBOperationException()
    {
        // Arrange
        var request = new CustomerUpdateRequest { Id = Guid.NewGuid(), FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123" };
        _mockCustomerService.Setup(s => s.UpdateCustomerAsync(request)).ThrowsAsync(new DBOperationException("DB error"));

        // Act
        var result = await _controller.UpdateCustomer(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteCustomer_Returns500_WhenDBOperationException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockCustomerService.Setup(s => s.DeleteCustomerAsync(id)).ThrowsAsync(new DBOperationException("DB error"));

        // Act
        var result = await _controller.DeleteCustomer(id);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}