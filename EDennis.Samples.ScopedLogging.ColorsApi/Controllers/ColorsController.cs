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

    internal class AutoJson
    {
        private readonly dynamic _data;
        public AutoJson(dynamic data) { _data = data; }
        public override string ToString() => JToken.FromObject(_data).ToString();
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase {
        private readonly ColorDbContext _context;
        private ILogger _logger;

        internal string P { get { return HttpContext.Request.Path; } }
        internal string C { get { return HttpContext.Request.RouteValues["controller"].ToString(); } }
        internal string A { get { return HttpContext.Request.RouteValues["action"].ToString(); } }
        internal string U { get { return HttpContext.User?.Identity?.Name ?? "unknown"; } }
        internal AutoJson R(dynamic data) => new AutoJson(data);

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
            _logger.LogInformation("For {User}, {Path}", U, P);
            _logger.LogDebug("For {User}, {Controller}.{Action}",U,C,A);

            var colors = await _context.Color.ToListAsync();

            _logger.LogTrace("For {User}, {Controller}.{Action}\n\tReturning: {Return}", U,C,A,R(colors));

            return colors;
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            _logger.LogInformation("For {User}, {Path}", U, P);
            _logger.LogDebug("For {User}, {Controller}.{Action}\n\tid={id}", U, C, A, id);

            var color = await _context.Color.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            _logger.LogTrace("For {User}, {Controller}.{Action}\n\tid={id}\n\tReturning: {Return}", U, C, A, id, R(color));

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
