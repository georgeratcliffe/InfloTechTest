using UserManagement.Models;
using UserManagement.Shared.ViewModels;

namespace UserManagement.Shared.Helpers;
public class Helpers
{
    public static UserListItemViewModel GetModel(User user)
    {
        return (
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

    public static User GetUser(UserListItemViewModel model)
    {
        return (
            new User()
            {
                Id = model.Id,
                Forename = model.Forename ?? string.Empty,
                Surname = model.Surname ?? string.Empty,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email ?? string.Empty,
                IsActive = model.IsActive
            });
    }
}
