using AutoMapper;
using RestaurantManager.API.DTOs.Request;
using RestaurantManager.API.DTOs.Response;
using RestaurantManager.Domain.Entities;

namespace RestaurantManager.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ── Restaurant ──
        CreateMap<CreateRestaurantRequest, Restaurant>();
        CreateMap<UpdateRestaurantRequest, Restaurant>();
        CreateMap<Restaurant, RestaurantResponse>();

        // ── Table ──
        CreateMap<CreateTableRequest, Table>();
        CreateMap<UpdateTableRequest, Table>();
        CreateMap<Table, TableResponse>()
            .ForMember(dest => dest.RestaurantName,
                opt => opt.MapFrom(src =>
                    src.Restaurant != null ? src.Restaurant.Name : string.Empty));

        // ── Customer ──
        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<UpdateCustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();

        // ── MenuItem ──
        CreateMap<CreateMenuItemRequest, MenuItem>();
        CreateMap<UpdateMenuItemRequest, MenuItem>();
        CreateMap<MenuItem, MenuItemResponse>();

        // ── Reservation ──
        CreateMap<CreateReservationRequest, Reservation>();
        CreateMap<UpdateReservationRequest, Reservation>();
        CreateMap<Reservation, ReservationResponse>()
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src =>
                    src.Customer != null ? src.Customer.FullName : string.Empty))
            .ForMember(dest => dest.TableNumber,
                opt => opt.MapFrom(src =>
                    src.Table != null ? src.Table.Number : 0))
            .ForMember(dest => dest.RestaurantName,
                opt => opt.MapFrom(src =>
                    src.Table != null && src.Table.Restaurant != null
                        ? src.Table.Restaurant.Name : string.Empty));
    }
}