using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Retrieve secret key for JWT from appsettings.json
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

// Add controllers to the services
builder.Services.AddControllers();

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        // Allow specific origins
        builder.WithOrigins("https://your-frontend-domain.com")  // Replace with your frontend domain
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Swagger Configuration with JWT support
builder.Services.AddSwaggerGen(options =>
{
    // Add JWT Bearer Security Definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR...\""
    });

    // Add global security requirement for Swagger
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// Add support for endpoints
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Enable Swagger in Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS middleware
app.UseCors(); // Enable CORS with the default policy

// Redirect HTTP requests to HTTPS (ensure proper certificate is used)
app.UseHttpsRedirection(); // MUST be called before authentication

// Configure authentication and authorization middleware
app.UseAuthentication(); // Authentication must come before Authorization
app.UseAuthorization();

// Map the controllers to routes
app.MapControllers();

// Run the application
app.Run();
