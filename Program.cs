using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"));
});

// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException(
        "JWT Key is not configured.");

var jwtIssuer = jwtSettings["Issuer"]
    ?? throw new InvalidOperationException(
        "JWT Issuer is not configured.");

var jwtAudience = jwtSettings["Audience"]
    ?? throw new InvalidOperationException(
        "JWT Audience is not configured.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)),

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    });

// Authorization
builder.Services.AddAuthorization();

// Repository
// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();



builder.Services.AddScoped<
    IPlacementOfficerRepository,
    PlacementOfficerRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<
    IPlacementOfficerService,
    PlacementOfficerService>();
// College Repository
builder.Services.AddScoped<
    ICollegeRepository,
    CollegeRepository>();

// College Service
builder.Services.AddScoped<
    ICollegeService,
    CollegeService>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Role seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = services
        .GetRequiredService<UserManager<ApplicationUser>>();

    var configuration = services
        .GetRequiredService<IConfiguration>();

    // Create roles
    await RoleSeeder.SeedRolesAsync(roleManager);

    // Create initial SuperAdmin
    await RoleSeeder.SeedSuperAdminAsync(
        userManager,
        configuration);
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();