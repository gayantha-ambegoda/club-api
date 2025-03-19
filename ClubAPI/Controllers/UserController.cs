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
using ClubAPI.Services;
using ClubAPI.Extras;

namespace ClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JWTServices _jwtServices;

        public UserController(ApplicationDbContext context,JWTServices jwtServices)
        {
            _context = context;
            _jwtServices = jwtServices;
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ActionResult<User>> GetUser(SignInRequest request)
        {
            try
            {
                var response = await _jwtServices.Authenticate(request);
                if (response == null)
                {
                    return Unauthorized();
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<ActionResult<User>> SignUp(User user)
        {
            var request = new SignInRequest
            {
                Email = user.Email,
                Password = user.Password
            };
            user.Password = PasswordHasher.Hash(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            try
            {
                var response = await _jwtServices.Authenticate(request);
                if (response == null)
                {
                    return Unauthorized();
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
