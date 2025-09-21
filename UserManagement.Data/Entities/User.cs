using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

/// <summary>
/// Represents a user entity with personal and account information.
/// </summary>
public class User
{
	/// <summary>
	/// Gets or sets the unique identifier for the user.
	/// </summary>
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	/// <summary>
	/// Gets or sets the forename of the user.
	/// </summary>
	public string Forename { get; set; } = default!;

	/// <summary>
	/// Gets or sets the surname of the user.
	/// </summary>
	public string Surname { get; set; } = default!;

	/// <summary>
	/// Gets or sets the date of birth of the user.
	/// </summary>
	public DateTime DateOfBirth { get; set; }

	/// <summary>
	/// Gets or sets the email address of the user.
	/// </summary>
	public string Email { get; set; } = default!;

	/// <summary>
	/// Gets or sets a value indicating whether the user is active.
	/// </summary>
	public bool IsActive { get; set; }
}
