using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Shared.ViewModels;
using UserManagement.Shared.Helpers;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserAuditService _userAuditService;
    public UsersController(IUserService userService, IUserAuditService userAuditService) {
        _userService = userService;
        _userAuditService = userAuditService;
    }

    [HttpGet]
    public ViewResult List(bool? isActive)
    {
        IEnumerable<UserListItemViewModel> items;

        if (!isActive.HasValue)
            items = _userService.GetAll().Select(p => Helpers.GetModel(p));
        else
            items = _userService.FilterByActive((bool)isActive).Select(p => Helpers.GetModel(p));

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
            var user = Helpers.GetUser(model);

            await _userService.Add(user);

            //var userAudits = _userAuditService.GetAllAudit().ToList();
            return RedirectToAction(nameof(List));
        }
        return View(model);
    }

    [Route("Details/{id}")]
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserById((long)id);

        var userAudits = _userAuditService.GetUserAuditsByUserId((long)id).ToList();

        if (user == null)
        {
            return NotFound();
        }

        var userModel = Helpers.GetModel(user);

        UserDetailsViewModel model = new()
        {
            Item = userModel,
            AuditEntries = userAudits
        };

        return View(model);
    }

    [Route("Edit/{id}")]
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserById((long)id);

        if (user == null)
        {
            return NotFound();
        }

        var model = Helpers.GetModel(user);

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
                var user = Helpers.GetUser(model);

                _userService.Update(user);
                //var userAudits = _userAuditService.GetAllUserAudits().ToList();
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
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserById((long)id);

        if (user == null)
        {
            return NotFound();
        }

        var model = Helpers.GetModel(user);

        return View(model);
    }


    [HttpPost]
    [Route("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var user = await _userService.GetUserById((long)id);
        if (user != null)
        {
            await _userService.Delete(user);
        }

        return RedirectToAction(nameof(List));
    }


    [Route("Logs")]
    public ViewResult Logs()
    {
        var userAudits = _userAuditService.GetAllUserAudits().ToList();

        var model = new AuditLogsViewModel
        {
            AuditEntries = userAudits
        };

        return View(model);
    }
}
