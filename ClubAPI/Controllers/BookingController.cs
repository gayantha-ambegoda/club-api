using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClubAPI;
using ClubAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClubAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            return await _context.Booking.ToListAsync();
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpGet("GetByPart/{id}")]
        public async Task<ActionResult<List<Booking>>> GetBookingByPart(int id)
        {
            var booking = await _context.Booking.Where(x => x.FieldPartId == id).ToListAsync();

            return booking;
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Booking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Save")]
        public async Task<ActionResult<Booking>> PostBooking(BookingRequest booking)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type.Equals("ID"));
            var fieldParts = _context.Parts.First(x => x.Id == booking.FieldPartId);

            if(fieldParts != null)
            {
                fieldParts.booking.Add(new Booking()
                {
                    FieldPartId = booking.FieldPartId,
                    Date = booking.Date,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    status = booking.status,
                    UserId = Int32.Parse(userId.Value)
                });
            }
            await _context.SaveChangesAsync();

            return Ok(booking);
        }
    }
}
