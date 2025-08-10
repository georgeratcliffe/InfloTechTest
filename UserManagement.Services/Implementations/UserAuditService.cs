using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;
public class UserAuditService : IUserAuditService
{
    private readonly IDataContext _dataAccess;
    public UserAuditService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<AuditEntry> GetAllAudit() => _dataAccess.GetAll<AuditEntry>();
}
