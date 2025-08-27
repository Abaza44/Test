using MaterialManagementAPI.Data;
using MaterialManagementAPI.Models;
using MaterialManagementAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ========== Database Configuration ==========

// Add Entity Framework DbContext with SQL Server
builder.Services.AddDbContext<MaterialManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========== Identity Configuration ==========

// Add ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<MaterialManagementContext>()
.AddDefaultTokenProviders();

// ========== JWT Authentication Configuration ==========

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // Set to true in production
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ClockSkew = TimeSpan.Zero
    };
});

// ========== Authorization Configuration ==========

builder.Services.AddAuthorization(options =>
{
    // Define role-based policies
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
    options.AddPolicy("SalesAccess", policy => policy.RequireRole("Manager", "Sales"));
    options.AddPolicy("AccountantAccess", policy => policy.RequireRole("Manager", "Accountant"));
    options.AddPolicy("FinancialAccess", policy => policy.RequireRole("Manager", "Accountant"));
});

// ========== Service Registration ==========

// Add custom services
builder.Services.AddScoped<IFIFOInventoryService, FIFOInventoryService>();

// Add controllers
builder.Services.AddControllers();

// ========== CORS Configuration ==========

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ========== Swagger/OpenAPI Configuration ==========

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Material Management API",
        Version = "v1",
        Description = "API for Material Management System - Construction Materials Warehouse",
        Contact = new OpenApiContact
        {
            Name = "Material Management System",
            Email = "support@materialmgmt.com"
        }
    });

    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// ========== Logging Configuration ==========

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ========== Build Application ==========

var app = builder.Build();

// ========== Middleware Pipeline Configuration ==========

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Material Management API v1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

// Security headers
app.UseHttpsRedirection();

// CORS - must be before authentication and authorization
app.UseCors("AllowAll");

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Static files for uploaded images
app.UseStaticFiles();

// Map controllers
app.MapControllers();

// ========== Database Initialization ==========

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MaterialManagementContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure database is created
        context.Database.EnsureCreated();

        // Seed roles and default admin user
        await SeedDataAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database or seeding data");
    }
}

// ========== Start Application ==========

app.Run();

// ========== Helper Methods ==========

/// <summary>
/// Seeds initial roles and default admin user
/// </summary>
/// <param name="userManager">User manager instance</param>
/// <param name="roleManager">Role manager instance</param>
static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    // Create roles if they don't exist
    string[] roles = { "Manager", "Sales", "Accountant" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create default admin user if it doesn't exist
    var adminEmail = "admin@materialmgmt.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Administrator",
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Manager");
        }
    }
}
