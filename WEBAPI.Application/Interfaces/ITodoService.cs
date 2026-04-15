using WEBAPI.Application.Common;
using WEBAPI.Application.DTOs;

namespace WEBAPI.Application.Interfaces;

public interface ITodoService
{
    Task<ServiceResult<List<TodoItemDto>>> GetAllAsync();
    Task<ServiceResult<TodoItemDto>> GetByIdAsync(int id);
    Task<ServiceResult<TodoItemDto>> CreateAsync(CreateTodoRequest request);
    Task<ServiceResult<TodoItemDto>> UpdateAsync(UpdateTodoRequest request);
    Task<ServiceResult> DeleteAsync(int id);
}
