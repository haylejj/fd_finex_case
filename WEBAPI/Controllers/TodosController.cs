using Microsoft.AspNetCore.Mvc;
using WEBAPI.Application.DTOs;
using WEBAPI.Application.Interfaces;

namespace WEBAPI.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController(ITodoService todoService) : BaseController
{
    // Amaç: Tüm endpoint'lerde ortak olan CreateResult metodunu kullanarak, servis katmanýndan dönen ServiceResult nesnelerini standart bir API yanýt formatýna dönüþtürmek ve uygun HTTP durum kodlarýyla birlikte istemciye iletmektir.
    // Controllerý çok temiz çünkü business logic yok, sadece servis katmanýndan gelen sonuçlarý HTTP yanýtlarýna dönüþtürmekle görevli. Bu sayede Single Responsibility Principle'a uygun bir yapý saðlanmýþ oluyor.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await todoService.GetAllAsync();
        return CreateResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await todoService.GetByIdAsync(id);
        return CreateResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequest request)
    {
        var result = await todoService.CreateAsync(request);
        return CreateResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTodoRequest request)
    {
        var result = await todoService.UpdateAsync(request);
        return CreateResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await todoService.DeleteAsync(id);
        return CreateResult(result);
    }
}
