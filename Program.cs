using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Implementations;
using VehiclePartsIMS_Backend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

builder.Services
    .AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register application services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPartRequestService, PartRequestService>();
builder.Services.AddScoped<IServiceReviewService, ServiceReviewService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();