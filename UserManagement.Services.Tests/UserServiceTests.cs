using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public async Task GetUserById_MustReturnExpectedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsersForAsync();
        var expectedUserId = 2;

        // Act: Invokes the method under test with the arranged parameters.
        var newUsers = new List<User>()
        {
            new User() {Id = 1, Forename="Jon", Surname="Doe"},
            new User() {Id = 2, Forename="Jon2", Surname="Doe2"}
        };

        var result = await service.GetUserById(expectedUserId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users.SingleOrDefault(u => u.Id == expectedUserId));
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
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
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private IQueryable<User> SetupUsersForAsync()
    {
        var users = new[]
        {
            new User
            {
                Id = 1,
                Forename = "Jon",
                Surname = "Doe",
                Email = "jd@acme.com",
                IsActive = true
            },
            new User
            {
                Id= 2,
                Forename = "Tim",
                Surname = "Smith",
                Email = "ts@acme.com",
                IsActive = true
            }

        }.AsQueryable().BuildMock();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }


    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
