using Hospital_Management_System.Data.Repositories.Implementations;
using Hospital_Management_System.Data.Repositories.Interfaces;
using Hospital_Management_System.Middleware;
using Hospital_Management_System.Services.Implementations;
using Hospital_Management_System.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// custom dependency injection
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();


var app = builder.Build();

// Global Exception Handling (FIRST)
app.UseMiddleware<ExceptionMiddleware>();

// Logging Middleware
app.Use(async (context, next) =>
{
    var start = DateTime.Now;

    await next();

    var duration = DateTime.Now - start;

    Console.WriteLine($"{context.Request.Method} {context.Request.Path} took {duration.TotalMilliseconds}ms");
});
app.UseMiddleware<LoggingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
