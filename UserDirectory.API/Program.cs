using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserDirectory.API.Data;
using UserDirectory.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuration: connection string from appsettings.json or default to /data/app.db
var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=/data/app.db";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(conn));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: JWT Bearer authentication (uncomment and configure if needed)
// var jwtSection = builder.Configuration.GetSection("Jwt");
// if (jwtSection.Exists())
// {
//     builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//         .AddJwtBearer(options =>
        // {
        //     options.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true,
        //         ValidIssuer = jwtSection["Issuer"],
        //         ValidAudience = jwtSection["Audience"],
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]))
        //     };
        // });
    // builder.Services.AddAuthorization();
    // }

var app = builder.Build();

// Ensure database created and optionally seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" },
            new User { FirstName = "Bob", LastName = "Jones", Email = "bob@example.com" }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// If you enable JWT, enable the authentication/authorization middleware
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
