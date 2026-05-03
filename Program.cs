// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using VehiclePartsIMS_Backend.Data;
// using VehiclePartsIMS_Backend.Data.Entities;
// using VehiclePartsIMS_Backend.Services;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
//
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
// builder.Services.AddSwaggerGen();
//
// builder.Services.AddDbContext<AppDbContext>(
//     options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
// );
// builder.Services
//     .AddIdentity<User, IdentityRole<int>>()
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddDefaultTokenProviders();
//
// builder.Services.AddScoped<IInvoiceService, InvoiceService>();
// builder.Services.AddScoped<IPartRequestService, PartRequestService>();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
// app.UseAuthorization();
//
// app.MapControllers();
//
// app.Run();


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services;
using VehiclePartsIMS_Backend.Services.Implementations;
using VehiclePartsIMS_Backend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3001",   // your frontend
                "http://localhost:5173",   // Vite default
                "http://localhost:3000",   // CRA / other
                "http://localhost:4173"    // Vite preview
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);
builder.Services
    .AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPartRequestService, PartRequestService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// ── Middleware order matters: CORS must come before Auth & Controllers ─────────
app.UseCors("FrontendDev");
app.UseAuthentication();   // added — Identity needs this before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();