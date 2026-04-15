using AutoMapper;
using WEBAPI.Application.DTOs;
using WEBAPI.Domain.Entities;

namespace WEBAPI.Application.Mappings;

public class TodoMappingProfile : Profile
{
    // manuel mapleme yapmak yerine otomatik mapleme yaparak kod tekrarýný azaltmak ve mapping iþlemlerini merkezi bir yerde yönetmek için AutoMapper kütüphanesini kullandým.
    // Bu sayede yeni bir mapping eklemek istediðimde sadece bu profile'a ekleme yapmam yeterli oluyor. 
    public TodoMappingProfile()
    {
        CreateMap<TodoItem, TodoItemDto>();

        CreateMap<CreateTodoRequest, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()));

        CreateMap<UpdateTodoRequest, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()));
    }
}
