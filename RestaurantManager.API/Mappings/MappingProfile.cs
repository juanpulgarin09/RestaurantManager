using AutoMapper;
using RestaurantManager.Domain.DTOs.Request;
using RestaurantManager.Domain.DTOs.Response;
using RestaurantManager.Domain.Entities;

namespace RestaurantManager.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Restaurant
        CreateMap<Restaurant, RestaurantResponse>();
        CreateMap<CreateRestaurantRequest, Restaurant>();
        CreateMap<UpdateRestaurantRequest, Restaurant>();

    }
}