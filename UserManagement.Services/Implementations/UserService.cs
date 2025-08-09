using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;



    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive) => _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive);
    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public Task Add(User user) => _dataAccess.Create(user);
    public Task<User?> GetUserById(long id) => _dataAccess.GetAll<User>().SingleOrDefaultAsync(u => u.Id == id);
    public Task Update(User user) => _dataAccess.Update(user);
    public Task Delete(User user) => _dataAccess.Delete(user);
    public bool UserExists(long id) => _dataAccess.GetAll<User>().Any(u => u.Id == id);
    
}
