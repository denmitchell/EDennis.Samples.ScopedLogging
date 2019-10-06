using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EDennis.Samples.ScopedLogging.ColorsApi.Models;
using EDennis.AspNetCore.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly ColorDbContext _context;
        private ILogger _logger;
        public ColorsController(ColorDbContext context, 
            IEnumerable<ILogger<ColorsController>> loggers, 
            ScopeProperties scopeProperties)
        {
            _context = context;
            _logger = loggers.ElementAt(scopeProperties.LoggerIndex);
        }

        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            _logger.LogInformation("GetColors called by {User}",
                                HttpContext.User?.Identity?.Name ?? "unknown");

            var colors = await _context.Color.ToListAsync();

            _logger.LogTrace("GetColors called by {User} returning {Colors}",
                HttpContext.User?.Identity?.Name ?? "unknown",
                JToken.FromObject(colors).ToString());

            return colors;
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            _logger.LogInformation("GetColor({id}) called by {User}", id,
                                HttpContext.User?.Identity?.Name ?? "unknown");

            var color = await _context.Color.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            _logger.LogTrace("GetColor({id}) called by {User} returning {Color}",
                id,
                HttpContext.User?.Identity?.Name ?? "unknown",
                JToken.FromObject(color).ToString());

            return color;
        }

        // PUT: api/Colors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(int id, Color color)
        {
            if (id != color.Id)
            {
                return BadRequest();
            }

            _context.Entry(color).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColorExists(id))
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

        // POST: api/Colors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            _context.Color.Add(color);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColor", new { id = color.Id }, color);
        }

        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Color>> DeleteColor(int id)
        {
            var color = await _context.Color.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            _context.Color.Remove(color);
            await _context.SaveChangesAsync();

            return color;
        }

        private bool ColorExists(int id)
        {
            return _context.Color.Any(e => e.Id == id);
        }
    }
}
