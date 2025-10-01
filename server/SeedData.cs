using WebSqliteApp.Models;
using WebSqliteApp.Services;

namespace WebSqliteApp;

public static class SeedData
{
    public static void Seed(AppDb db)
    {
        if (!db.Users.Any())
        {
            db.Users.Add(new User { Email = "admin@example.com", PasswordHash = PasswordHasher.Hash("admin123") });
            db.SaveChanges();
            Console.WriteLine("Usuario admin creado: admin@example.com / admin123");
        }
    }
}
