using QikApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Qik API",
        Version = "v1",
        Description = "REST API for the Qik template generation library. Qik provides powerful script-based template generation with support for variables, expressions, conditional logic, and text transformation functions.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Qik on GitHub",
            Url = new Uri("https://github.com/rob-bl8ke/Qik")
        }
    });

    // Include XML comments for better documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Register Qik service
builder.Services.AddSingleton<IQikService, QikService>();

// Configure CORS for Angular frontend
builder.Services.AddCors(options =>
{
    // Allow specific origin (Angular frontend)
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    // Allow all origins (for development/testing only)
    // Uncomment if you need to allow requests from any origin
    // options.AddPolicy("AllowAll", policy =>
    // {
    //     policy.AllowAnyOrigin()
    //           .AllowAnyMethod()
    //           .AllowAnyHeader();
    // });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Qik API v1");
        // options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
