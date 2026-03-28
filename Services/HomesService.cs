using AutoMapper;
using CityApi.Data;
using CityApi.Dtos;
using FirtsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CityApi.Services
{
    public class HomesService
    {
        private readonly CityDBContext _dbContext;
        private readonly IMapper _mapper;

        public HomesService(CityDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<Apartment>> GetApartmentsByHomeAsync(Guid homeId)
        {
            var apartments = await _dbContext.Apartments
                .Where(x => x.HomeId == homeId)
                .Include(x => x.Residents)
                .ToListAsync();

            var apartmentDtos = _mapper.Map<List<ApartmentDto>>(apartments);
            return apartments;
        }
    }
}
