using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Controllers;
using WebApplication1.Extensions;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services
                .AddSwaggerConfig()
                .InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);




// Add services to the container.

builder.ConfigureCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureSwaggerExplorer()
    .AddIdentityAuthMiddlewares();



app.MapControllers();


app.UseCors("AllowAngularApp");
app
    .MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapIdentityUserEndpoints()
    .MapAccountEndpoints();

app.Run();


