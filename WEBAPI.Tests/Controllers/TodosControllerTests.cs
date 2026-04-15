using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WEBAPI.Application.Common;
using WEBAPI.Application.DTOs;
using WEBAPI.Application.Interfaces;
using WEBAPI.Controllers;

namespace WEBAPI.Tests.Controllers;

public class TodosControllerTests
{
    private readonly Mock<ITodoService> _todoServiceMock = new();
    private readonly TodosController _controller;

    public TodosControllerTests()
    {
        _controller = new TodosController(_todoServiceMock.Object);
    }

    // Amaç: Liste endpoint'inin başarılı durumda 200 kodu ve dolu veri dönmesini doğrulamak.
    [Fact]
    public async Task GetAll_ReturnsOkServiceResult()
    {
        var items = new List<TodoItemDto>
        {
            new() { Id = 1, Title = "Gorev 1", IsCompleted = false, CreatedAt = DateTime.UtcNow }
        };

        _todoServiceMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(ServiceResult<List<TodoItemDto>>.Success(items, HttpStatusCode.OK));

        var actionResult = await _controller.GetAll();
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        var result = Assert.IsType<ServiceResult<List<TodoItemDto>>>(objectResult.Value);

        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data!);
    }

    // Amaç: Kayıt bulunamadığında GetById endpoint'inin 404 ve hata mesajı dönmesini doğrulamak.
    [Fact]
    public async Task GetById_ReturnsNotFoundServiceResult_WhenItemMissing()
    {
        _todoServiceMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync(ServiceResult<TodoItemDto>.Failure("Todo kaydi bulunamadi.", HttpStatusCode.NotFound));

        var actionResult = await _controller.GetById(99);
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        var result = Assert.IsType<ServiceResult<TodoItemDto>>(objectResult.Value);

        Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        Assert.False(result.IsSuccess);
        Assert.Contains("Todo kaydi bulunamadi.", result.ErrorList!);
    }

    // Amaç: Create endpoint'inin başarılı oluşturma senaryosunda 201 dönmesini doğrulamak.
    [Fact]
    public async Task Create_ReturnsCreatedServiceResult()
    {
        var request = new CreateTodoRequest { Title = "Yeni Gorev" };
        var createdItem = new TodoItemDto
        {
            Id = 10,
            Title = "Yeni Gorev",
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _todoServiceMock
            .Setup(x => x.CreateAsync(request))
            .ReturnsAsync(ServiceResult<TodoItemDto>.SuccessAsCreated(createdItem, "/api/todos/10"));

        var actionResult = await _controller.Create(request);
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        var result = Assert.IsType<ServiceResult<TodoItemDto>>(objectResult.Value);

        Assert.Equal((int)HttpStatusCode.Created, objectResult.StatusCode);
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Data!.Id);
    }

    // Amaç: Update endpoint'inin geçerli istekte 200 dönüp güncel veriyi taşıdığını doğrulamak.
    [Fact]
    public async Task Update_ReturnsOkServiceResult()
    {
        var request = new UpdateTodoRequest
        {
            Id = 3,
            Title = "Guncel Gorev",
            IsCompleted = true
        };

        var updatedItem = new TodoItemDto
        {
            Id = 3,
            Title = "Guncel Gorev",
            IsCompleted = true,
            CreatedAt = DateTime.UtcNow
        };

        _todoServiceMock
            .Setup(x => x.UpdateAsync(request))
            .ReturnsAsync(ServiceResult<TodoItemDto>.Success(updatedItem, HttpStatusCode.OK));

        var actionResult = await _controller.Update(request);
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        var result = Assert.IsType<ServiceResult<TodoItemDto>>(objectResult.Value);

        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        Assert.True(result.IsSuccess);
        Assert.True(result.Data!.IsCompleted);
    }

    // Amaç: Delete endpoint'inin başarılı silme senaryosunda 204 dönmesini doğrulamak.
    [Fact]
    public async Task Delete_ReturnsNoContentServiceResult()
    {
        _todoServiceMock
            .Setup(x => x.DeleteAsync(5))
            .ReturnsAsync(ServiceResult.Success(HttpStatusCode.NoContent));

        var actionResult = await _controller.Delete(5);
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        var result = Assert.IsType<ServiceResult>(objectResult.Value);

        Assert.Equal((int)HttpStatusCode.NoContent, objectResult.StatusCode);
        Assert.True(result.IsSuccess);
    }
}
