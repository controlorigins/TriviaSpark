using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Core.Services;
using TriviaSpark.Web.Areas.Admin.Models;

namespace TriviaSpark.Web.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class UserRolesController(
    ApplicationUserManager userManager, 
    RoleManager<IdentityRole> roleManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await userManager.Users.ToListAsync();
        var userRolesViewModel = new List<UserRolesViewModel>();
        foreach (Core.Entities.TriviaSparkWebUser user in users)
        {
            var thisViewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = await GetUserRoles(user)
            };
            userRolesViewModel.Add(thisViewModel);
        }
        return View(userRolesViewModel);
    }

    public async Task<IActionResult> Edit(string id)
    {
        ViewBag.userId = id;
        var user = await userManager.GetUserModelById(id);
        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
            return View("NotFound");
        }
        await ValidateRoles(id);

        ViewBag.UserName = user.UserName;
        return View(user);
    }


    public async Task<IActionResult> Manage(string userId)
    {
        ViewBag.userId = userId;
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
            return View("NotFound");
        }
        await ValidateRoles(userId);

        ViewBag.UserName = user.UserName;
        var model = new List<ManageUserRolesViewModel>();
        foreach (var role in roleManager.Roles)
        {
            var userRolesViewModel = new ManageUserRolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }
            model.Add(userRolesViewModel);
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
    {

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return View();
        }
        var roles = await userManager.GetRolesAsync(user);
        var result = await userManager.RemoveFromRolesAsync(user, roles);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Cannot remove user existing roles");
            return View(model);
        }
        result = await userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Cannot add selected roles to user");
            return View(model);
        }
        return RedirectToAction("Index");
    }
    private async Task<List<string>> GetUserRoles(Core.Entities.TriviaSparkWebUser user)
    {
        return new List<string>(await userManager.GetRolesAsync(user));
    }

    private async Task ValidateRoles(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        var CurrentRoles = await userManager.GetRolesAsync(user);

        if (CurrentRoles.Count < 1)
        {
            await roleManager.CreateAsync(new IdentityRole(Core.Entities.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Core.Entities.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Core.Entities.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Core.Entities.Roles.Basic.ToString()));
        }
    }

}
