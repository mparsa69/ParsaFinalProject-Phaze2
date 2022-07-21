using App.Domain.Core.ApplicationUserAgg.Entities;
using App.EndPoints.Web.Mvc.Areas.Admin.Models.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace App.EndPoints.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = ConstantProperty.Role_Admin)]
    public class UserManagementController : Controller
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string? name)
        {

            List<UserManagementVM> model;
            if (string.IsNullOrEmpty(name))
            {
                model = await _userManager.Users.Select(x => new UserManagementVM
                {
                    Id = x.Id,
                    Name = x.UserName,
                    //FirstName = x.FirstName,
                    //LastName=x.Family,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                }).ToListAsync();
            }
            else
            {
                model = await _userManager.Users
                    .Where(x => x.UserName.Contains(name))
                    .Select(x => new UserManagementVM
                    {
                        Id = x.Id,
                        Name = x.UserName,
                        //FirstName = x.FirstName,
                        //LastName = x.Family,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                    }).ToListAsync();
            }

            foreach (var user in model)
            {
                user.Roles =
                    await _userManager.GetRolesAsync(await _userManager.Users.FirstAsync(x => x.Id == user.Id));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            var model = new UserManagementVM()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = await _userManager.GetRolesAsync(user)
            };
            var allRoles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            var selectedRoles = allRoles.Where(x => !model.Roles.Contains(x)).ToList();
            
            ViewBag.AllRoles = selectedRoles;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserRoles(string userId,string role)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("UserRoles", new {userId=userId});

        }
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId,string role)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            await _userManager.RemoveFromRoleAsync(user, role);
            return RedirectToAction("UserRoles", new { userId = userId });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveUser(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            var model = new UserManagementVM
            {
                Id = user.Id,
                Name = user.UserName,
                //FirstName = user.FirstName,
                //LastName = user.Family,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserManagementVM model)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == model.Id);
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Name;
            //user.FirstName = model.FirstName;
            //user.Family = model.LastName;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        /*public IActionResult UserRoles()
        {
            return View()
        }
        ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager();

        string userId = User.Identity.GetUserId();

        // get user roles
        List<string> roles = userManager.GetRoles(userId).ToList();*/

    }
}
