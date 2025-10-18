using System.ComponentModel.DataAnnotations;

namespace WebSqliteApp.Models;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public class StudentDto
{
    [Required, MaxLength(100)] public string Nombre { get; set; } = string.Empty;   
    [Required, MaxLength(100)] public string Apellido {  get; set; } = string.Empty;
    [Required, MaxLength(100), EmailAddress] public string Email { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
}

