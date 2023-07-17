using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Clear any existing log providers, if any
builder.Logging.ClearProviders();

// Configure Logging Format
builder.Logging.AddConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "dd-MM-yyy HH:mm:ss";
    options.DisableColors = false;
});

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swift Message Reader", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swift Message Reader");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
