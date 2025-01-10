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
    public class WorkoutPlansController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkoutPlansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkoutPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutPlan>>> GetWorkoutPlans()
        {
            return await _context.WorkoutPlans.ToListAsync();
        }

        // GET: api/WorkoutPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutPlan>> GetWorkoutPlan(int id)
        {
            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            return workoutPlan;
        }

        // PUT: api/WorkoutPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkoutPlan(int id, WorkoutPlan workoutPlan)
        {
            if (id != workoutPlan.WorkoutPlanId)
            {
                return BadRequest();
            }

            _context.Entry(workoutPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutPlanExists(id))
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

        // POST: api/WorkoutPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkoutPlan>> PostWorkoutPlan(WorkoutPlan workoutPlan)
        {
            _context.WorkoutPlans.Add(workoutPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkoutPlan", new { id = workoutPlan.WorkoutPlanId }, workoutPlan);
        }

        // DELETE: api/WorkoutPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int id)
        {
            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            _context.WorkoutPlans.Remove(workoutPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkoutPlanExists(int id)
        {
            return _context.WorkoutPlans.Any(e => e.WorkoutPlanId == id);
        }

        [HttpGet]
        public IActionResult GetWorkoutPlans(
    [FromQuery] int page = 1,
    [FromQuery] int size = 10,
    [FromQuery] string sortBy = "Name",
    [FromQuery] string order = "asc",
    [FromQuery] int? trainerId = null) // Filtering by TrainerId
        {
            var query = _context.WorkoutPlans.AsQueryable();

            if (trainerId.HasValue)
            {
                query = query.Where(wp => wp.TrainerId == trainerId.Value);
            }

            if (order.ToLower() == "asc")
            {
                query = query.OrderBy(wp => EF.Property<object>(wp, sortBy));
            }
            else
            {
                query = query.OrderByDescending(wp => EF.Property<object>(wp, sortBy));
            }

            var totalRecords = query.Count();
            var workoutPlans = query
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = page,
                PageSize = size,
                WorkoutPlans = workoutPlans
            });
        }

    }
}
