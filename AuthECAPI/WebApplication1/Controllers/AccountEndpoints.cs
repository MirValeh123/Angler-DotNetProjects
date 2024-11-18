using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/UserProfile", GetUserProfile);
            return builder;
        }

        [Authorize]
        private static string GetUserProfile()
        {
            return "User profile";
        }
    }

}
