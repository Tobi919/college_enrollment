using FluentValidation;
using WebSqliteApp.Models;

namespace WebSqliteApp.Validators;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(500);
    }
}

public class EnrollmentDtoValidator : AbstractValidator<EnrollmentDto>
{
    public EnrollmentDtoValidator()
    {
        RuleFor(x => x.StudentId).GreaterThan(0);
        RuleFor(x => x.CourseId).GreaterThan(0);
    }
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class StudentDtoValidator : AbstractValidator<StudentDto>
{
    public StudentDtoValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Apellido).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.FechaNacimiento).LessThanOrEqualTo(DateTime.Today).When(x => x.FechaNacimiento.HasValue);
    }
}