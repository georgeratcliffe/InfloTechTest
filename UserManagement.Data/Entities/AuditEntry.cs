using System;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Data.Entities;
public class AuditEntry
{
    public Guid Id { get; set; }

    public long UserId { get; set; }
    public string? Metadata { get; set; } = string.Empty;

    public string Forename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public EntityState EntityState { get; set; }
}
