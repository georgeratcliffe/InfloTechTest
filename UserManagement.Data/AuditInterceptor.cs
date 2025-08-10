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

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        #pragma warning disable CS8602
        var auditEntries = eventData.Context.ChangeTracker.Entries()
            .Where(x => x.Entity is not AuditEntry && x.State is EntityState.Added
            or EntityState.Modified or EntityState.Deleted)
            .Select(x =>
                new AuditEntry
                {
                    Id = Guid.CreateVersion7(),
                    Metadata = x.DebugView.LongView,
                    UserId = ((User)x.Entity).Id,
                    Forename = ((User)x.Entity).Forename,
                    Surname = ((User)x.Entity).Surname,
                    Email = ((User)x.Entity).Email,
                    DateOfBirth = ((User)x.Entity).DateOfBirth,
                    IsActive = ((User)x.Entity).IsActive,
                    StartTime = DateTime.UtcNow,
                    EntityState = x.State
                }
            ).ToList();

        #pragma warning restore CS8602

        if (auditEntries.Count ==0)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        _auditEntries.AddRange(auditEntries);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
        

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {

        if (eventData.Context is null)
        {
            await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        foreach (var auditEntry in _auditEntries)
        {
            auditEntry.EndTime = DateTime.UtcNow;
        }

        #pragma warning disable CS8602
        if (_auditEntries.Count > 0)
        {
            eventData.Context?.Set<AuditEntry>().AddRange(_auditEntries);
            _auditEntries.Clear();
            await eventData.Context.SaveChangesAsync();
        }
        #pragma warning restore CS8602


        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
