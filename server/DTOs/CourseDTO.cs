namespace WebSqliteApp.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
}