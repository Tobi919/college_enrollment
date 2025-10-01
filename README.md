# .NET 8 + Controllers + Swagger + FluentValidation + SQLite
**BackEnd:** ASP.NET Core (.NET 8) con **Controllers**, **JWT**, **Swagger (XML comments)**, **FluentValidation**  
**FrontEnd:** HTML/CSS/JS puro (servido por el back)  
**DB:** SQLite (`data/app.db`, se crea automáticamente)


## 1) Instalar paquetes y ejecutar..
```bash
cd server
dotnet restore
dotnet run
```
- Front: `http://localhost:5000/`
- Swagger: `http://localhost:5000/swagger`
- Login: `admin@example.com` / `admin123`

## 2) Rutas (versión v1 por path)
- Auth: `/api/v1/auth/login`, `/api/v1/auth/me`
- Students: `/api/v1/students`
- Courses: `/api/v1/courses`
- Enrollments: `/api/v1/enrollments`

## 3) Búsqueda y paginación
Query params: `q`, `page` (≥1), `pageSize` (1–50). Respuesta:
```json
{ "total": 123, "page": 1, "pageSize": 10, "items": [ ... ] }
```

## 4) Validaciones
- **DataAnnotations** en DTOs
- **FluentValidation** en `server/Validators/*` (registro automático).

## 5) Estructura
```
server/
├─ Controllers/
│  ├─ AuthController.cs
│  ├─ StudentsController.cs
│  ├─ CoursesController.cs
│  └─ EnrollmentsController.cs
├─ Models/
│  ├─ AppDb.cs, User.cs, Student.cs, Course.cs, Enrollment.cs, Dtos.cs
├─ Validators/
│  └─ Validators.cs
├─ Services/
│  ├─ JwtService.cs
│  └─ PasswordHasher.cs
├─ Program.cs
client/ (HTML/CSS/JS)
data/ (app.db)
```
