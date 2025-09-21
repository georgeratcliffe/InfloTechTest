using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

/// <summary>
/// Provides user management operations such as retrieval, creation, update, and deletion.
/// </summary>
public class UserService : IUserService
{
	private readonly IDataContext _dataAccess;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserService"/> class.
	/// </summary>
	/// <param name="dataAccess">The data context used for data operations.</param>
	public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

	/// <summary>
	/// Returns users filtered by their active state.
	/// </summary>
	/// <param name="isActive">The active state to filter by.</param>
	/// <returns>An enumerable of users matching the active state.</returns>
	public IEnumerable<User> FilterByActive(bool isActive) => _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive);

	/// <summary>
	/// Retrieves all users.
	/// </summary>
	/// <returns>An enumerable of all users.</returns>
	public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

	/// <summary>
	/// Retrieves a user by their unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the user.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
	public Task<User?> GetUserById(long id) => _dataAccess.GetAll<User>().SingleOrDefaultAsync(u => u.Id == id);

	/// <summary>
	/// Adds a new user.
	/// </summary>
	/// <param name="user">The user to add.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task Add(User user) => _dataAccess.Create(user);

	/// <summary>
	/// Updates an existing user.
	/// </summary>
	/// <param name="user">The user to update.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task Update(User user) => _dataAccess.Update(user);

	/// <summary>
	/// Deletes a user.
	/// </summary>
	/// <param name="user">The user to delete.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task Delete(User user) => _dataAccess.Delete(user);

	/// <summary>
	/// Determines whether a user exists by their unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the user.</param>
	/// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
	public bool UserExists(long id) => _dataAccess.GetAll<User>().Any(u => u.Id == id);
}
