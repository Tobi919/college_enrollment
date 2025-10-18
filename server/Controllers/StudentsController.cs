using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using WebSqliteApp.Models;

namespace WebSqliteApp.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDb _db;
        public StudentsController(AppDb db) { _db = db; }


        //consultar un objeto por ID
        [HttpGet("{id:int}")]  // localhost:8080/api/v1/students/1234
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id) //=> _db.Students.Find(id) is Student s ? Ok(s) : BadRequest();
        {
            try
            {
                var student = _db.Students.SingleOrDefault(x => x.Id == id);
                return Ok(student);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            //return _db.Students.Find(id) is Student s ? Ok(s) : BadRequest();
        }

        //Obtener listado de objetos
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)] //localhost:8080/api/v1/students?filtro=Juan&page=0&pageSize=0
        [ProducesResponseType(400)]
        public IActionResult GetAll([FromQuery] string? filtro, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                page = page > 0 ? page : 1;
                pageSize = pageSize > 0 ? pageSize : 10;

                var query = _db.Students.AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {
                    var like = $"%{filtro}%";
                    var like2 = "%" + like + "%";

                    query = query.Where(s =>
                    EF.Functions.Like(s.Nombre, like) ||
                    EF.Functions.Like(s.Apellido, like) ||
                    EF.Functions.Like(s.Email, like));
                }

                var total = query.Count();
                var items = query.ToList().OrderBy(s => s.Nombre).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Ok(new { total, page, pageSize, items });

            }
            catch
            {
                return BadRequest();
            }
        }

        //Crear un objeto
        [HttpPost]
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(400)]
        public IActionResult Create([FromBody] StudentDto dto)
        {
            var s = new Student { Nombre = dto.Nombre, Apellido = dto.Apellido, Email = dto.Email, FechaNacimiento = dto.FechaNacimiento };
            _db.Students.Add(s);
            try
            {
                _db.SaveChanges();
                return Ok(s);
            }
            catch
            {
                return BadRequest("Email duplicado u otro error.");
            }
        }

        //Editar un objeto
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult Update(int id, [FromBody] StudentDto dto)
        {
            var s = _db.Students.Find(id);
            if(s is null)
            {
                return NotFound();
            }

            s.Nombre = dto.Nombre;
            s.Apellido = dto.Apellido;
            s.Email = dto.Email;
            
            try
            {
                _db.SaveChanges();
                return Ok(s);
            }
            catch
            {
                return BadRequest("Email duplicado u otro error");
            }
        }

        //Eliminar un objeto
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult Delete(int id) 
        {
            var s = _db.Students.Find(id);
            if (s is null)
            {
                return NotFound();
            }

            try
            {
                _db.Remove(s);
                _db.SaveChanges();
                return Ok(new {ok = true});
            }
            catch
            {
                return BadRequest("Ocurrio algun error");
            }
        }

    }
}
