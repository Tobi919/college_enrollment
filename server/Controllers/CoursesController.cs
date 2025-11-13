using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSqliteApp.Models;

namespace WebSqliteApp.Controllers;

/// <summary>CRUD de cursos (con búsqueda y paginación).</summary>
[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDb _db;
    public CoursesController(AppDb db) { _db = db; }

    //consultar un objeto por ID
    [HttpGet("{id:int}")]  // localhost:8080/api/v1/students/1234
    [ProducesResponseType(typeof(Student), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id) //=> _db.Students.Find(id) is Student s ? Ok(s) : BadRequest();
    {
        try
        {
            var course = _db.Courses.SingleOrDefault(x => x.Id == id);
            return Ok(course);

        }
        catch (Exception ex)
        {
            return BadRequest();
        }

        //return _db.Students.Find(id) is Student s ? Ok(s) : BadRequest();
    }

    /// <summary>Lista cursos con búsqueda y paginación.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), 200)]
    public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        page = page > 0 ? page : 1;
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = _db.Courses.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q}%";
            query = query.Where(c => EF.Functions.Like(c.Nombre, like) || EF.Functions.Like(c.Descripcion!, like));
        }

        var total = query.Count();
        var items = query.OrderByDescending(c => c.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return Ok(new { total, page, pageSize, items });
    }

    /// <summary>Crea un curso.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Course), 200)]
    [ProducesResponseType(400)]
    public IActionResult Create([FromBody] CourseDto dto)
    {
        var c = new Course { Nombre = dto.Nombre, Descripcion = dto.Descripcion };
        _db.Courses.Add(c); _db.SaveChanges(); return Ok(c);
    }

    /// <summary>Actualiza un curso.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Course), 200)]
    [ProducesResponseType(404)]
    public IActionResult Update(int id, [FromBody] CourseDto dto)
    {
        var c = _db.Courses.Find(id); if (c is null) return NotFound();
        c.Nombre = dto.Nombre; c.Descripcion = dto.Descripcion; _db.SaveChanges(); return Ok(c);
    }

    /// <summary>Elimina un curso.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult Delete(int id)
    {
        var c = _db.Courses.Find(id); if (c is null) return NotFound();
        _db.Courses.Remove(c); _db.SaveChanges(); return Ok(new { ok = true });
    }
}
