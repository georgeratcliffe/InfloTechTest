using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(bool? isActive)
    {
        IEnumerable<UserListItemViewModel> items;

        if (!isActive.HasValue)
        {
            items = _userService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                DateOfBirth = p.DateOfBirth,
                Email = p.Email,
                IsActive = p.IsActive
            });
        }
        else
        {
            items = _userService.FilterByActive((bool)isActive).Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                DateOfBirth = p.DateOfBirth,
                Email = p.Email,
                IsActive = p.IsActive
            });
        }

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [Route("Create")]
    public ViewResult Create()
    {
        return View();
    }

    [HttpPost]
    [Route("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Forename,Surname,DateOfBirth,Email,IsActive")] UserListItemViewModel model)
    {
        await Task.CompletedTask;

        if (ModelState.IsValid)
        {
            var user = new User()
            {
                Forename = model.Forename ?? string.Empty,
                Surname = model.Surname ?? string.Empty,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email ?? string.Empty,
                IsActive = model.IsActive
            };

            _userService.Add(user);
            return RedirectToAction(nameof(List));
        }
        return View(model);
    }

    [Route("Details/{id}")]
    public IActionResult Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _userService.GetUserById((long)id);

        if (user == null)
        {
            return NotFound();
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [Route("Edit/{id}")]
    public IActionResult Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _userService.GetUserById((long)id);

        if (user == null)
        {
            return NotFound();
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [Route("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(long id, [Bind("Id,Forename,Surname,DateOfBirth,Email,IsActive")] UserListItemViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var user = new User()
                {
                    Id= model.Id,
                    Forename = model.Forename ?? string.Empty,
                    Surname = model.Surname ?? string.Empty,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email ?? string.Empty,
                    IsActive = model.IsActive
                };
                _userService.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_userService.UserExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(List));
        }
        return View(model);
    }


    [Route("Delete/{id}")]
    public IActionResult Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _userService.GetUserById((long)id);
        //var user = await _context.User
        //    .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return View(model);
    }


    [HttpPost]
    [Route("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(long id)
    {
        var user = _userService.GetUserById((long)id);
        if (user != null)
        {
            _userService.Delete(user);
        }

        return RedirectToAction(nameof(List));
    }

}
