using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityApi.Data;
using FirtsApi.Models;
using AutoMapper;
using CityApi.Dtos;
using Elasticsearch.Net;

namespace CityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentsController : ControllerBase
    {
        private readonly CityDBContext _context;
        private readonly IMapper _mapper;

        public ResidentsController(CityDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/Residents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResidentDto>>> GetResidents()
        {
            var resident = await _context.Residents.ToListAsync();
            var residentDt = _mapper.Map<List<ResidentDto>>(resident);
            return Ok(residentDt);
        }

        //public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetApartmentsByHomeId(Guid homeId)
        //{
        //    var apartments = await _context.Apartments
        //        .Where(a => a.HomeId == homeId)
        //        .ToListAsync();

        //    return Ok(_mapper.Map<IEnumerable<ApartmentDto>>(apartments));
        //}

        // GET: api/Residents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResidentDto>> GetResident(Guid id)
        {
            var resident = await _context.Residents.FindAsync(id);
            var residentDto = _mapper.Map<ResidentDto>(resident);

            if (residentDto == null)
            {
                return NotFound();
            }

            return residentDto;
        }

        // PUT: api/Residents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> PutResident(Guid id, [FromBody] ResidentDto residentUpdateDTO)
        {
            if (id != residentUpdateDTO.Id)
            {
                return BadRequest();
            }

            var resident = _mapper.Map<Resident>(residentUpdateDTO);
            _context.Entry(resident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResidentExists(id))
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

        // POST: api/Residents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResidentDto>> PostResident(ResidentDto residentCreateDto)
        {
            var residents = _mapper.Map<Resident>(residentCreateDto);
            _context.Residents.Add(residents);
            await _context.SaveChangesAsync();

            var residentDto = _mapper.Map<ResidentDto>(residents);

            return CreatedAtAction("GetResident", new { id = residentDto.Id }, residentDto);
        }

        // DELETE: api/Residents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResident(Guid id)
        {
            var resident = await _context.Residents.FindAsync(id);
            if (resident == null)
            {
                return NotFound();
            }

            _context.Residents.Remove(resident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResidentExists(Guid id)
        {
            return _context.Residents.Any(e => e.Id == id);
        }
    }
}
