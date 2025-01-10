using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fitness_Tracker.Data;
using Fitness_Tracker.Models;

namespace Fitness_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
        {
            return await _context.Trainers.ToListAsync();
        }

        // GET: api/Trainers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
            {
                return NotFound();
            }

            return trainer;
        }

        // PUT: api/Trainers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainer(int id, Trainer trainer)
        {
            if (id != trainer.TrainerId)
            {
                return BadRequest();
            }

            _context.Entry(trainer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerExists(id))
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

        // POST: api/Trainers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Trainer>> PostTrainer(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainer", new { id = trainer.TrainerId }, trainer);
        }

        // DELETE: api/Trainers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.TrainerId == id);
        }

        [HttpGet]
        public IActionResult GetTrainers(
       [FromQuery] int page = 1,          // Pagination: Page number
       [FromQuery] int size = 10,         // Pagination: Page size
       [FromQuery] string sortBy = "Name", // Sorting: Field to sort by
       [FromQuery] string order = "asc",  // Sorting: asc/desc
       [FromQuery] string expertise = null // Filtering: Expertise
   )
        {
            // Base query
            var query = _context.Trainers.AsQueryable();

            // Filtering by expertise
            if (!string.IsNullOrEmpty(expertise))
            {
                query = query.Where(t => t.Expertise.Contains(expertise));
            }

            // Sorting
            if (order.ToLower() == "asc")
            {
                query = query.OrderBy(t => EF.Property<object>(t, sortBy));
            }
            else
            {
                query = query.OrderByDescending(t => EF.Property<object>(t, sortBy));
            }

            // Pagination
            var totalRecords = query.Count();
            var trainers = query
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            // Return paginated response
            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = page,
                PageSize = size,
                Trainers = trainers
            });
        }
    }
}
