using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Shared.ViewModels;

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
        }).WithName("User");


        app.MapPost("api/CreateUser/", async (UserListItemViewModel model, IUserService userService) =>
        {
            UserListItemViewModel? createdUserModel = new();
            User? createdUser = new();
            
            try
            {
                var user = GetUser(model);
                await userService.Add(user);
                createdUser = userService.GetAll().OrderByDescending(u => u.Id).FirstOrDefault() ?? new User();
                createdUserModel = GetModel(createdUser);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

            return Results.CreatedAtRoute("User", routeValues: new { id = createdUser.Id }, value: createdUserModel);
        });

        app.MapPut("api/EditUser/{id}", async (long id, UserListItemViewModel model, IUserService userService) =>
        {
            UserListItemViewModel? editedUserModel = new();
            User? editedUser = new();

            var user = await userService.GetUserById(id);
            if (user == null) return Results.NotFound($"User Id {id} not found");

            try
            {
                user.Forename = model.Forename ?? string.Empty;
                user.Surname = model.Surname ?? string.Empty;
                user.Email = model.Email ?? string.Empty;
                user.DateOfBirth = model.DateOfBirth;
                user.IsActive = model.IsActive;
                await userService.Update(user);
                editedUser = await userService.GetUserById(id) ?? new User();
                editedUserModel = GetModel(editedUser);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

            return Results.Ok(editedUserModel);
        });


        app.MapDelete("api/DeleteUser/{id}", async (long id, IUserService userService) =>
        {
            var user = await userService.GetUserById(id);
            if (user == null) return Results.NotFound($"User Id {id} not found");

            try
            {
                await userService.Delete(user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

            return Results.Ok($"User {id} deleted");
        });


        app.MapGet("api/GetAllUserAudits", (IUserAuditService userAuditService) =>
        {
            var userAudits = userAuditService.GetAllUserAudits().ToList();

            return Results.Ok(userAudits);
        });


        app.MapGet("api/GetUserAuditByUserId/{id}", (long id, IUserAuditService userAuditService) =>
        {
            var userAudits = userAuditService.GetUserAuditsByUserId((long)id).ToList();
            return Results.Ok(userAudits);
        });


        return app;
    }


    public static User GetUser(UserListItemViewModel model) => (
        new User()
        {
            Id = model.Id,
            Forename = model.Forename ?? string.Empty,
            Surname = model.Surname ?? string.Empty,
            DateOfBirth = model.DateOfBirth,
            Email = model.Email ?? string.Empty,
            IsActive = model.IsActive
        });

    public static UserListItemViewModel GetModel(User user) =>
    (
        new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive
        });
 }

