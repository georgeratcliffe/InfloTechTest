using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Shared.ViewModels;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    #region Create
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Create_WhenModelStateIsInvalid_MustReturnCorrectModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();
        var expectedUserId = 1;
        var model = new UserListItemViewModel { Id = expectedUserId };
        controller.ModelState.AddModelError("error", "some error");


        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Create(model) as ViewResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result?.Model
            .Should().BeOfType<UserListItemViewModel>()
            .Which.Id.Should().Be(expectedUserId);
    }

    [Fact]
    public async Task Create_WhenModelStateIsValid_MustReturnCorrectModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();
        var model = new UserListItemViewModel();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Create(model) as ViewResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result?.Model
            .Should().BeOfType<UserListViewModel>();
    }
    #endregion

    #region Details

    [Fact]
    public async Task Details_WhenUserIdIsNull_MustReturnNotFoundResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Details(null) as IActionResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.GetType().Should().Be(typeof(NotFoundResult));
    }

    [Fact]
    public async Task Details_WhenUserIsNotFound_MustReturnNotFoundResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();
        var userId = 5;

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Details(userId) as IActionResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.GetType().Should().Be(typeof(NotFoundResult));
    }

    [Fact]
    public async Task Details_WhenUserFound_MustReturnCorrectModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var userId = 5;

        #pragma warning disable CS8620 
        _userService.Setup(s => s.GetUserById(It.IsAny<long>())).Returns(Task.FromResult(new User()));
        #pragma warning restore CS8620

        var controller = CreateController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Details(userId) as ViewResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result?.Model
            .Should().BeOfType<UserDetailsViewModel>();
    }

    #endregion

    #region Edit

    [Fact]
    public async Task Edit_WhenUserIsNotFound_MustReturnNotFoundResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Edit(null) as IActionResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.GetType().Should().Be(typeof(NotFoundResult));
    }

    [Fact]
    public async Task Edit_WhenUserFound_MustReturnCorrectModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var userId = 5;

        #pragma warning disable CS8620
        _userService.Setup(s => s.GetUserById(It.IsAny<long>())).Returns(Task.FromResult(new User()));
        #pragma warning restore CS8620

        var controller = CreateController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Edit(userId) as ViewResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result?.Model
            .Should().BeOfType<UserListItemViewModel>();
    }

    [Fact]
    public async Task Edit_WhenUserIsNotSameAsModel_MustReturnNotFoundResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();
        var model = new UserListItemViewModel();
        var userId = 5;

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Edit(userId, model) as IActionResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.GetType().Should().Be(typeof(NotFoundResult));
    }

    [Fact]
    public async Task Edit_WhenModelStateIsValid_MustReturnCorrectModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var userId = 5;
        var model = new UserListItemViewModel() { Id = userId };


        _userService.Setup(s => s.Update(It.IsAny<User>())).Returns(Task.CompletedTask);

        var controller = CreateController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Edit(userId,model) as ViewResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result?.Model
            .Should().BeOfType<UserListViewModel>();
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_WhenUserIsNotFound_MustReturnNotFoundResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(null) as IActionResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.GetType().Should().Be(typeof(NotFoundResult));
    }

    [Fact]
    public async Task Delete_WhenUserFound_MustReturnCorrectModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var userId = 5;

        #pragma warning disable CS8620
        _userService.Setup(s => s.GetUserById(It.IsAny<long>())).Returns(Task.FromResult(new User()));
#pragma warning restore CS8620

        var controller = CreateController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(userId) as ViewResult;

        // Assert: Verifies that the action of the method under test behaves as expected.
        result?.Model
            .Should().BeOfType<UserListItemViewModel>();
    }

    #endregion

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<IUserAuditService> _userAuditService = new();
    private UsersController CreateController() => new(_userService.Object, _userAuditService.Object);

}
