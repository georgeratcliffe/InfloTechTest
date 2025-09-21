using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
	/// <summary>
	/// Filters users by their active status.
	/// </summary>
	/// <param name="isActive">True to filter active users; false for inactive users.</param>
	/// <returns>An enumerable collection of users matching the active status.</returns>
	IEnumerable<User> FilterByActive(bool isActive);
	/// <summary>
	/// Retrieves all users.
	/// </summary>
	/// <returns>An enumerable collection of all users.</returns>
	IEnumerable<User> GetAll();
	/// <summary>
	/// Gets a user by their unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the user.</param>
	/// <returns>The user if found; otherwise, null.</returns>
	Task<User?> GetUserById(long id);
	/// <summary>
	/// Adds a new user.
	/// </summary>
	/// <param name="user">The user to add.</param>
	Task Add(User user);
	/// <summary>
	/// Updates an existing user.
	/// </summary>
	/// <param name="user">The user with updated information.</param>
	Task Update(User user);
	/// <summary>
	/// Deletes a user by their unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the user to delete.</param>
	Task Delete(User id);
	/// <summary>
	/// Checks if a user exists by their unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the user.</param>
	/// <returns>True if the user exists; otherwise, false.</returns>
	bool UserExists(long id);
}
