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

namespace CityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly CityDBContext _context;
        private readonly IMapper _mapper;

        public ApartmentsController(CityDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/Apartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetApartments()
        {
            var apartment = await _context.Apartments.ToListAsync();
            var ApartamentD = _mapper.Map<List<ApartmentDto>>(apartment);
            return Ok(ApartamentD);
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentDto>> GetApartment(Guid id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            var ApartamentDto = _mapper.Map<ApartmentDto>(apartment);
            if (ApartamentDto == null)
            {
                return NotFound();
            }

            return ApartamentDto;
        }



        // PUT: api/Apartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> PutApartment(Guid id, [FromBody] ApartmentDto apartmentUpdateDTO)
        {
            if (id != apartmentUpdateDTO.Id)
            {
                return BadRequest();
            }

            var apartment = _mapper.Map<Apartment>(apartmentUpdateDTO);
            _context.Entry(apartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(id))
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




        // POST: api/Apartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApartmentDto>> PostApartment(ApartmentDto apartmentDto)
        {
            var apartmentsD = _mapper.Map<Apartment>(apartmentDto);
            _context.Apartments.Add(apartmentsD);
            await _context.SaveChangesAsync();

            var apartmentDTO = _mapper.Map<ApartmentDto>(apartmentsD);

            return CreatedAtAction("GetApartment", new { id = apartmentDTO.Id }, apartmentDTO);
        }

        // DELETE: api/Apartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(Guid id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApartmentExists(Guid id)
        {
            return _context.Apartments.Any(e => e.Id == id);
        }
    }
}
