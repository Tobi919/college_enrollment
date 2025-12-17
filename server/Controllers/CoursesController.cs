using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSqliteApp.Models;
using WebSqliteApp.DTOs;

namespace WebSqliteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDb _context;

        public CoursesController(AppDb context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Select(static c => new CourseDto { Id = c.Id, Name = c.Nombre, Nombre = c.Nombre })
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new CourseDto { Id = c.Id, Name = c.Nombre, Nombre = c.Nombre })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto courseDTO)
        {
            var course = new Course { Nombre = courseDTO.Name, Enrollments = new List<Enrollment>() };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            courseDTO.Id = course.Id;
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, courseDTO);
        }

        // PUT: api/courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDto courseDTO)
        {
            if (id != courseDTO.Id)
                return BadRequest();

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            course.Nombre = courseDTO.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}