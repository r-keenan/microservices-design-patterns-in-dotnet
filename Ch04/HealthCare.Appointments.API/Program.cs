using HealthCare.Appointments.API.Constants;
using HealthCare.Appointments.API.Models;
using HealthCare.Appointments.API.Repositories;
using HealthCare.Appointments.API.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("AppointmentsDatabaseConnection");
builder.Services.AddDbContext<AppointmentsDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});
builder.Services.AddControllers();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ApiEndpoints>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

builder.Services.AddTransient(typeof(IHttpRepository<>), typeof(HttpRepository<>));
builder.Services.AddTransient<IDoctorsApiRepository, DoctorsApiRepository>();
builder.Services.AddTransient<IPatientsApiRepository, PatientsApiRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
