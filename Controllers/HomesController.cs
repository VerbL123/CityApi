using AutoMapper;
using CityApi.Data;
using CityApi.Dtos;
using CityApi.Services;
using FirtsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomesController : ControllerBase
    {
        private readonly CityDBContext _context;
        private readonly IMapper _mapper;
        private readonly HomesService _homesService;

        public HomesController(
            CityDBContext context,
            IMapper mapper,
            HomesService homesService)
        {
            _context = context;
            _mapper = mapper;
            _homesService = homesService;

        }

        // GET: api/Homes
        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<HomeDto>>> GetHomes()
        {
            var homes = await _context.Homes.ToListAsync();
            var homeDtos = _mapper.Map<List<HomeDto>>(homes);
            return homeDtos;
        }
        //
        [HttpGet("{homeId}/apartments")]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetApartmentsByHomeId(Guid homeId)
        {
            var apartmentsG = await _context.Apartments.Where(a => a.HomeId == homeId).ToListAsync();

            if (!apartmentsG.Any())
            {
                return NotFound();
            }

            return Ok(apartmentsG);
        }

        // GET: api/Homes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeDto>> GetHome(Guid id)
        {
            var home = await _context.Homes.FindAsync(id);
            var homeDt = _mapper.Map<HomeDto>(home);
            if (homeDt == null)
            {
                return NotFound();
            }

            return homeDt;
        }


        // PUT: api/Homes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHome(Guid id, [FromBody] HomeDto homeUpdateDTO)
        {
            var homeExist = await HomeExists(id);

            if (!homeExist)
            {
                return BadRequest();
            }

            var home = _mapper.Map<Home>(homeUpdateDTO);
            home.Id = id;
            _context.Entry(home).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!homeExist)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Homes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HomeDto>> CreateHome(HomeDto homeCreateDTO)
        {
            var home = _mapper.Map<Home>(homeCreateDTO);

            _context.Homes.Add(home);
            await _context.SaveChangesAsync();

            var homeDTO = _mapper.Map<HomeDto>(home);

            return CreatedAtAction(nameof(GetHome), new { id = homeDTO.Id }, homeDTO);
        }

        // DELETE: api/Homes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHome(Guid id)
        {
            var home = await _context.Homes.FindAsync(id);
            if (home == null)
            {
                return NotFound();
            }

            _context.Homes.Remove(home);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> HomeExists(Guid id)
        {
            return await _context.Homes.AnyAsync(e => e.Id == id);
        }
    }
}
