using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB"));
});


// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = 
    x.DefaultChallengeScheme = 
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(y =>
{
    y.SaveToken = false;
    y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();



app.UseCors("AllowAngularApp");
app
    .MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapPost("/api/signup", async (UserManager<AppUser> userManager,[FromBody]UserRegistrationModel userRegistrationModel ) =>
{
    AppUser user = new AppUser()
    {
        UserName = userRegistrationModel.Email,
        Email = userRegistrationModel.Email,
        FullName = userRegistrationModel.FullName,
    };
    var result =await userManager.CreateAsync(user,userRegistrationModel.Password);
    if(result.Succeeded)
        return Results.Ok(result);
    else
        return Results.BadRequest(result);
});


app.MapPost("/api/signin", async (UserManager<AppUser> userManager, [FromBody] LoginModel login ) => 
{
    var user = await userManager.FindByEmailAsync(login.Email);
    if (user != null && await userManager.CheckPasswordAsync(user,login.Password))
    {
        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim("UserID",user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(
                signInKey,
                SecurityAlgorithms.HmacSha256Signature
                )
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return Results.Ok(new {token});
    }
    else
    {
        return Results.BadRequest( new
        {
            message = "Username or password is incorrect."
        });
    }

});
app.Run();


public class UserRegistrationModel
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }
}

public class LoginModel
{
    public string Email { get; set; }

    public string Password { get; set; }

}