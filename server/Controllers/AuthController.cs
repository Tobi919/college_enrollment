using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSqliteApp.Models;
using WebSqliteApp.Services;

namespace WebSqliteApp.Controllers;

/// <summary>
/// Autenticación con JWT.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDb _db;
    private readonly JwtService _jwt;
    public AuthController(AppDb db, JwtService jwt) { _db = db; _jwt = jwt; }

    /// <summary>Autentica y devuelve un token JWT.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _db.Users.SingleOrDefault(u => u.Email == dto.Email);
        if (user is null || !PasswordHasher.Verify(dto.Password, user.PasswordHash)) return Unauthorized();
        var token = _jwt.GenerateToken(user.Email, user.Id.ToString());
        return Ok(new { token });
    }

    /// <summary>Valida el token y devuelve el email del usuario.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    public IActionResult Me()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        return Ok(new { ok = true, email });
    }
}
