public class Student
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
}