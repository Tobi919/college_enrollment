using System.Diagnostics.Contracts;

namespace WebSqliteApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public DateTime? FechaNacimiento { get; set; }
        public string NumeroTelefono { get; set; } = string.Empty;


    }
}
