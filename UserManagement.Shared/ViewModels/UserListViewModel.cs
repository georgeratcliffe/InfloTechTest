
using System.ComponentModel.DataAnnotations;
using UserManagement.Data.Entities;

namespace UserManagement.Shared.ViewModels;

/// <summary>
/// View model representing a list of users.
/// </summary>
public class UserListViewModel
{
	/// <summary>
	/// Gets or sets the collection of user list items.
	/// </summary>
	public List<UserListItemViewModel> Items { get; set; } = new();
}

/// <summary>
/// View model representing detailed information about a user, including audit entries.
/// </summary>
public class UserDetailsViewModel
{
	/// <summary>
	/// Gets or sets the user list item details.
	/// </summary>
	public UserListItemViewModel Item { get; set; } = new();

	/// <summary>
	/// Gets or sets the collection of audit entries for the user.
	/// </summary>
	public List<AuditEntry> AuditEntries { get; set; } = new();
}

/// <summary>
/// View model representing audit logs.
/// </summary>
public class AuditLogsViewModel
{
	/// <summary>
	/// Gets or sets the collection of audit entries.
	/// </summary>
	public List<AuditEntry> AuditEntries { get; set; } = new();
}

/// <summary>
/// View model representing a single user item in a list.
/// </summary>
public class UserListItemViewModel
{
	/// <summary>
	/// Gets or sets the unique identifier of the user.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// Gets or sets the forename of the user.
	/// </summary>
	[Required]
	public string? Forename { get; set; }

	/// <summary>
	/// Gets or sets the surname of the user.
	/// </summary>
	[Required]
	public string? Surname { get; set; }

	/// <summary>
	/// Gets or sets the date of birth of the user.
	/// </summary>
	public DateTime DateOfBirth { get; set; }

	/// <summary>
	/// Gets or sets the email address of the user.
	/// </summary>
	public string? Email { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the user is active.
	/// </summary>
	public bool IsActive { get; set; }
}

