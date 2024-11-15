using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public static class IdentityUserEndpoints
    {
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
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/signup", CreateUser);


            app.MapPost("/api/signin", SignIn);
            return app;
        }


        private static async Task<IResult> CreateUser(UserManager<AppUser> userManager, [FromBody] UserRegistrationModel userRegistrationModel)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegistrationModel.Email,
                Email = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
            };
            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);
            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);
        }

        private static async Task<IResult> SignIn(UserManager<AppUser> userManager, [FromBody] LoginModel login,IOptions<AppSettings> options)
        {
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, login.Password))
            {
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.JWTSecret));

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
                return Results.Ok(new { token });
            }
            else
            {
                return Results.BadRequest(new
                {
                    message = "Username or password is incorrect."
                });
            }
        }

    }
}
