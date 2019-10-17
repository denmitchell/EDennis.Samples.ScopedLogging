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
    public class Colors2Controller : ControllerBase {

        private readonly ColorRepo _repo;
        public Colors2Controller(ColorRepo repo)
        {
            _repo = repo;
        }

        // GET: api/Colors
        [HttpGet]
        public ActionResult<IEnumerable<Color>> GetColors()
        {
            return Ok(_repo.GetColors());
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            var color = await _repo.GetColor(id);

            if (color == null)
            {
                return NotFound();
            }

            return Ok(color);
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


            try
            {
                await _repo.UpdateColor(color);
            } catch (DbUpdateConcurrencyException)
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
            var result = await _repo.CreateColor(color);

            return CreatedAtAction("GetColor", new { id = color.Id }, result);
        }

        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Color>> DeleteColor(int id)
        {
            var color = await _repo.DeleteColor(id);
            if (color == null)
            {
                return NotFound();
            }

            return color;
        }

        private bool ColorExists(int id)
        {
            return _repo.ColorExists(id);
        }
    }
}
