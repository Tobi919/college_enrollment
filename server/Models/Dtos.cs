using System.ComponentModel.DataAnnotations;

namespace WebSqliteApp.Models;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

