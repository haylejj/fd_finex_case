namespace WEBAPI.Application.DTOs;

public class CreateTodoRequest
{
    // id veritabaný üzerinden otomatik üretiliyor.
    public string Title { get; set; } = string.Empty;
}
