using System;
using System.ComponentModel.DataAnnotations;
using UserManagement.Data.Entities;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserDetailsViewModel
{
    public UserListItemViewModel Item { get; set; } = new();
    public List<AuditEntry> AuditEntries { get; set; } = new();
}

public class AuditLogsViewModel
{
    public List<AuditEntry> AuditEntries { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }
    [Required]
    public string? Forename { get; set; }
    [Required]
    public string? Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}
