using UserManagement.Services.Domain.Interfaces;

namespace UserManager.API.Extensions;

public static class Extensions
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("api/GetAllUsers", (IUserService userService) =>
        {
            var users = userService.GetAll().ToList();

            return Results.Ok(users);
        });

        app.MapGet("api/GetUserByUserId/{id}", async (long id,IUserService userService) =>
        {
            var user = await userService.GetUserById(id);
            return user == null ? Results.NotFound($"User Id {id} not found") : Results.Ok(user);
        });

        return app;
    }
   
   
}
