using System.Collections.Generic;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Interfaces;
public interface IUserAuditService
{
    IEnumerable<AuditEntry> GetAllAudit();
    IEnumerable<AuditEntry> GetUserAuditsByUserId(long id);
}
