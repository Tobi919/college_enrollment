namespace WebSqliteApp.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Email { get; set; }
    public DateTime? FechaNacimiento { get; set; }
}