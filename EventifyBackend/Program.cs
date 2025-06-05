using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventifyBackend.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<EventifyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.")))
        };
    });

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eventify Backend API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Initialize and migrate the database at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EventifyDbContext>();
    try
    {
        // Ensure the database is created if it doesn't exist
        dbContext.Database.EnsureCreated();
        Console.WriteLine("Database created or already exists.");

        // Apply any pending migrations
        dbContext.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully.");

        // Test database connection
        var canConnect = dbContext.Database.CanConnect();
        Console.WriteLine($"Database connection test: {(canConnect ? "Success" : "Failed")}");

        // Verify table count (optional)
        var tableCount = dbContext.Database.SqlQueryRaw<int>("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public'").FirstOrDefault();
        Console.WriteLine($"Number of tables in public schema: {tableCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database setup error: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eventify Backend API v1");
    c.RoutePrefix = "swagger"; // Serve Swagger UI at /swagger
});

// Comment out HTTPS redirection for local testing
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Add a simple test endpoint
app.MapGet("/", () => "Eventify Backend is running!");

app.Run();