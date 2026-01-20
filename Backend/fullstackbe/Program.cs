using fullstackbe.Core.Application;
using fullstackbe.Core.Domain;
using fullstackbe.Gateways.Data;
using fullstackbe.Gateways.Repository;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Opret implementationer i IOC container
builder.Services.AddScoped<IVirksomhedRepository, VirksomhedRepository>();
builder.Services.AddScoped<IVirksomhedCrud, VirksomhedCrud>();

// GraphQl

builder.Services.AddGraphQLServer();
builder.AddGraphQL().AddTypes();

// SQLite, check lige hvor filen havner
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=.\\data\\database.db"));

// TODO Begræns adgang tillad alt mht. CORS
builder.Services.AddCors(options => {
    options.AddPolicy(
        name: "default",
        builder => {
            builder.WithOrigins("https://localhost:5095;http://localhost:5095")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)  // Vigtig
                .AllowCredentials();
        });
});
var app = builder.Build();

app.UseCors("default");

app.MapGraphQL();

app.RunWithGraphQLCommands(args);