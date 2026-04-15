using System.Net;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Application.Common;
using WEBAPI.Application.DTOs;
using WEBAPI.Application.Interfaces;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Data;

namespace WEBAPI.Infrastructure.Services;

public class TodoService(
    AppDbContext dbContext,
    IMapper mapper,
    IValidator<CreateTodoRequest> createValidator,
    IValidator<UpdateTodoRequest> updateValidator) : ITodoService
{
    // Tum gorevleri olusturma tarihine gore tersten listeler.
    public async Task<ServiceResult<List<TodoItemDto>>> GetAllAsync()
    {
        var items = await dbContext.TodoItems
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        // automapper ile entity'leri DTO'lara ceviriyoruz.
        var mappedItems = mapper.Map<List<TodoItemDto>>(items);
        return ServiceResult<List<TodoItemDto>>.Success(mappedItems, HttpStatusCode.OK);
    }

    // Id'ye gore tek bir gorev kaydi dondurur.
    public async Task<ServiceResult<TodoItemDto>> GetByIdAsync(int id)
    {
        var item = await dbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
        // eðer kayýt bulunamazsa 404 ve hata mesajý döneriz.
        if (item is null)
        {
            return ServiceResult<TodoItemDto>.Failure("Todo kaydi bulunamadi.", HttpStatusCode.NotFound);
        }

        var mappedItem = mapper.Map<TodoItemDto>(item);
        return ServiceResult<TodoItemDto>.Success(mappedItem, HttpStatusCode.OK);
    }

    // Yeni görev oluþturur ve olusan kaydi geri doner.
    public async Task<ServiceResult<TodoItemDto>> CreateAsync(CreateTodoRequest request)
    {
        // gelen istegin validasyonunu yapariz, eger hatalar varsa 400 ve hata mesajlari doneriz.
        var validationResult = await createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return ServiceResult<TodoItemDto>.Failure(
                validationResult.Errors.Select(x => x.ErrorMessage).Distinct().ToList(),
                HttpStatusCode.BadRequest);
        }

        var entity = mapper.Map<TodoItem>(request);

        dbContext.TodoItems.Add(entity);
        await dbContext.SaveChangesAsync();
        // kayit olustuktan sonra entity'nin Id'si otomatik olarak set edilir, bu yüzden mapper ile DTO'ya cevirirken Id de gelmis olur.
        var mappedItem = mapper.Map<TodoItemDto>(entity);
        return ServiceResult<TodoItemDto>.SuccessAsCreated(mappedItem, $"/api/todos/{entity.Id}");
    }

    // Mevcut görevi gunceller ve guncel halini doner.
    public async Task<ServiceResult<TodoItemDto>> UpdateAsync(UpdateTodoRequest request)
    {
        var validationResult = await updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return ServiceResult<TodoItemDto>.Failure(
                validationResult.Errors.Select(x => x.ErrorMessage).Distinct().ToList(),
                HttpStatusCode.BadRequest);
        }

        var existingItem = await dbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (existingItem is null)
        {
            return ServiceResult<TodoItemDto>.Failure("Todo kaydi bulunamadi.", HttpStatusCode.NotFound);
        }

        mapper.Map(request, existingItem);

        await dbContext.SaveChangesAsync();

        var mappedItem = mapper.Map<TodoItemDto>(existingItem);
        return ServiceResult<TodoItemDto>.Success(mappedItem, HttpStatusCode.OK);
    }

    // Id'ye gore gorev kaydini siler.
    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var existingItem = await dbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
        if (existingItem is null)
        {
            return ServiceResult.Failure("Todo kaydi bulunamadi.", HttpStatusCode.NotFound);
        }

        dbContext.TodoItems.Remove(existingItem);
        await dbContext.SaveChangesAsync();
        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

}
