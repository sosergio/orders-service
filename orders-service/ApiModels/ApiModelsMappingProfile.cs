using AutoMapper;

namespace OrdersService.Api.ApiModels
{
    public class ApiModelsMappingProfile : Profile
    {
        public ApiModelsMappingProfile()
            : base("ApiModelsMappingProfile")
        {
            CreateMap<Core.Models.Order, ApiModels.Order>().ReverseMap();
            CreateMap<Core.Models.OrderItem, ApiModels.OrderItem>().ReverseMap();
        }   
    }
}