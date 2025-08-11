using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;
public class UserAuditService : IUserAuditService
{
    private readonly IDataContext _dataAccess;
    public UserAuditService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<AuditEntry> GetAllUserAudits() => _dataAccess.GetAll<AuditEntry>();
    public IEnumerable<AuditEntry> GetUserAuditsByUserId(long id) => _dataAccess.GetAll<AuditEntry>().Where(ua => ua.UserId == id);
}
