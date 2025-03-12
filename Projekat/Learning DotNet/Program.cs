using Domain.Repositories;
using Service;
using Service.Abstractions;
using Learning_DotNet.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformator>();

builder.Services.AddDbContextPool<RepositoryDbContext>(b => 
{
    var connectionString = builder.Configuration.GetConnectionString("Database"); 
    b.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
    {
        options.MetadataAddress = "http://localhost:7080/realms/learning/.well-known/openid-configuration";
        options.Authority = "http://localhost:7080/realms/learning";
        options.Audience = "account";
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
