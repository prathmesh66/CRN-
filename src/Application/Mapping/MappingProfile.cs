using AutoMapper; using CRN.ProductApi.Application.DTOs; using CRN.ProductApi.Domain.Entities;
namespace CRN.ProductApi.Application.Mapping;
public class MappingProfile : Profile
{
 public MappingProfile(){CreateMap<Item,ItemDto>(); CreateMap<Product,ProductDto>();}
}
