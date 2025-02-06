using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient(); // Add HttpClient for external API calls

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply middleware in correct order
app.UseCors("AllowAll");   // Apply CORS before routing
app.UseRouting();          // Enable routing
app.UseAuthorization();    // Apply Authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller-Home}/{action-Index}/{id?}");      // Map API controllers

app.Run();

