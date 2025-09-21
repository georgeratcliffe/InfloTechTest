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

    public UsersController(IUserService userService, IUserAuditService userAuditService)
    {
        _userService = userService;
        _userAuditService = userAuditService;
    }

    [HttpGet]
    public ViewResult List(bool? isActive)
    {
        var users = !isActive.HasValue
            ? _userService.GetAll()
            : _userService.FilterByActive(isActive.Value);

        var model = new UserListViewModel
        {
            Items = users.Select(Helpers.GetModel).ToList()
        };

        return View(model);
    }

    [Route("Create")]
    public ViewResult Create() => View();

    [HttpPost]
    [Route("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Forename,Surname,DateOfBirth,Email,IsActive")] UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = Helpers.GetUser(model);
        await _userService.Add(user);
        return RedirectToAction(nameof(List));
    }

    [Route("Details/{id}")]
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
            return NotFound();

        var user = await _userService.GetUserById(id.Value);
        if (user == null)
            return NotFound();

        var userAudits = _userAuditService.GetUserAuditsByUserId(id.Value).ToList();

        var model = new UserDetailsViewModel
        {
            Item = Helpers.GetModel(user),
            AuditEntries = userAudits
        };

        return View(model);
    }

    [Route("Edit/{id}")]
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
            return NotFound();

        var user = await _userService.GetUserById(id.Value);
        if (user == null)
            return NotFound();

        var model = Helpers.GetModel(user);
        return View(model);
    }

    [HttpPost]
    [Route("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("Id,Forename,Surname,DateOfBirth,Email,IsActive")] UserListItemViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var user = Helpers.GetUser(model);
            await _userService.Update(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_userService.UserExists(model.Id))
                return NotFound();
            throw;
        }
        return RedirectToAction(nameof(List));
    }

    [Route("Delete/{id}")]
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
            return NotFound();

        var user = await _userService.GetUserById(id.Value);
        if (user == null)
            return NotFound();

        var model = Helpers.GetModel(user);
        return View(model);
    }

    [HttpPost]
    [Route("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var user = await _userService.GetUserById(id);
        if (user != null)
            await _userService.Delete(user);

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
