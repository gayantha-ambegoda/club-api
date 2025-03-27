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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ClubAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClubsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Clubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs()
        {
            return await _context.Clubs.ToListAsync();
        }

        // GET: api/Clubs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Club>> GetClub(int id)
        {
            var club = await _context.Clubs.FirstOrDefaultAsync(i => i.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            return club;
        }

        [HttpPost("AssignUserToClub")]
        public async Task<ActionResult> SaveClubUser(UserRoleRequest role)
        {
            try
            {
                var users = _context.Users.Where(user => user.Email == role.UserEmail).ToList();
                if(users.Count == 1)
                {
                    var userRole = new UserRoles()
                    {
                        UserId = users[0].Id,
                        ClubId = role.ClubId,
                        Role = role.Role
                    };
                    _context.UserRoles.Add(userRole);
                    await _context.SaveChangesAsync();
                    return Ok("Success!");
                }
                else
                {
                    return NotFound("Cannot Find User");
                }
                
            }catch(Exception exception)
            {
                return StatusCode(500,exception.Message);
            }
        }

        [HttpGet("GetUsersByClub/{id}")]
        public async Task<ActionResult<IList<User>>> GetClubUsers(int id)
        {
            var users = _context.Users.Include(u => u.UserRole).Where(u => u.UserRole.ToList().Count > 0).ToList();
            var user2 = users.Where(usr =>
            {
                if (usr.UserRole.Where(rl => rl.ClubId == id).Count() > 0)
                {
                    return true;
                }
                else return false;
            }).ToList();
            return Ok(user2);
        }

        // POST: api/Clubs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Club>> PostClub(Club club)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type.Equals("ID"));
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            _context.UserRoles.Add(new UserRoles()
            {
                ClubId = club.Id,
                UserId = Int32.Parse(userId.Value),
                Role = "Super Admin"
            });
            await _context.SaveChangesAsync();

            return Ok(club);
        }

        // DELETE: api/Clubs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.Id == id);
        }
    }
}
