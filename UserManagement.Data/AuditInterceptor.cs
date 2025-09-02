using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UserManagement.Data.Entities;
using UserManagement.Models;

namespace UserManagement.Data;
public class AuditInterceptor : SaveChangesInterceptor
{
    private List<AuditEntry> _auditEntries = new();
    public AuditInterceptor(List<AuditEntry> auditEntries)
    {
        _auditEntries = auditEntries;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        // Only audit User entities that are Added, Modified, or Deleted
        var userEntries = context.ChangeTracker.Entries()
            .Where(x => x.Entity is User &&
                        x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        var auditEntries = new List<AuditEntry>();
        foreach (var entry in userEntries)
        {
            var user = (User)entry.Entity;
            auditEntries.Add(new AuditEntry
            {
                Id = Guid.CreateVersion7(),
                Metadata = entry.DebugView?.LongView,
                UserId = user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive,
                StartTime = DateTime.UtcNow,
                EntityState = entry.State
            });
        }

        if (auditEntries.Count == 0)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        _auditEntries.AddRange(auditEntries);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
        

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        // Set EndTime for all audit entries
        var now = DateTime.UtcNow;
        foreach (var auditEntry in _auditEntries)
            auditEntry.EndTime = now;

        // Persist audit entries if any exist
        if (_auditEntries.Count > 0)
        {
            context.Set<AuditEntry>().AddRange(_auditEntries);
            _auditEntries.Clear();
            await context.SaveChangesAsync(cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
