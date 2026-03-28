using AutoMapper;
using CityApi.Dtos;
using FirtsApi.Models;

namespace CityApi.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Home, HomeDto>().ReverseMap();
            this.CreateMap<Apartment, ApartmentDto>().ReverseMap();//ForMember(d => d.ResidentsQuantity, o => o.MapFrom(s => s.Residents.Count));
            this.CreateMap<Resident, ResidentDto>().ReverseMap();
            this.CreateMap<User, UserDtos>().ReverseMap();

        }
    }
}
