using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using WebSqliteApp;
using WebSqliteApp.Models;
using WebSqliteApp.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Ensure data dir exists
var dataDir = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "data"));
Directory.CreateDirectory(dataDir);

// EF Core + SQLite
var conn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=../data/app.db";
builder.Services.AddDbContext<AppDb>(opt => opt.UseSqlite(conn));

// Controllers + FluentValidation
builder.Services.AddControllers()
    .AddJsonOptions(o => { o.JsonSerializerOptions.PropertyNamingPolicy = null; });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// JWT Auth
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"] ?? "devsecret");
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

builder.Services.AddSingleton<JwtService>();

// Swagger + XML comments
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebSqliteApp API", Version = "v1" });
    var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Introduce **Bearer {tu token JWT}**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, new string[] {} } });
});

var app = builder.Build();

// DB init
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.EnsureCreated();
    SeedData.Seed(db);
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebSqliteApp API v1");
});

app.UseAuthentication();
app.UseAuthorization();

// Serve static client
var clientPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "client"));
app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(clientPath) });
app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(clientPath) });

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/index.html"));

app.Run();
