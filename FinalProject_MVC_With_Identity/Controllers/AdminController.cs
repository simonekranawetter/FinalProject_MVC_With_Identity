using FinalProject_MVC_With_Identity.Models;
using FinalProject_MVC_With_Identity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinalProject_MVC_With_Identity.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            return View(await _context.Profiles.Include(x => x.User).ToListAsync());
        }

        public async Task<IActionResult> Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public async Task<IActionResult> AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel roleViewModel)
        {
            await _roleManager.CreateAsync(new IdentityRole(roleViewModel.RoleName));
            return RedirectToAction("Roles", "Admin");
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
            var editRoleViewModel = new RoleViewModel
            {
                RoleName = role.Name,
            };
            return View(editRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel roleViewModel)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleViewModel.Id.ToString());
            var result = await _roleManager.SetRoleNameAsync(role, roleViewModel.RoleName);
            var updateResult = await _roleManager.UpdateAsync(role);

            return RedirectToAction("Roles", "Admin");
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Roles", "Admin");
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var rolesForUsers = await _userManager.GetRolesAsync(user);
            var role = rolesForUsers.FirstOrDefault();
            var roles = _roleManager.Roles.ToList();

            List<SelectListItem> selectRoles = new List<SelectListItem>();
            foreach (var identityRole in roles)
            {
                selectRoles.Add(new SelectListItem(identityRole.Name, identityRole.Name));
            }

            var profileEntity = await _context.Profiles.FirstOrDefaultAsync(p => p.Id.ToString() == id);
            var userProfile = new UserProfile
            {
                UserId = profileEntity.UserId,
                FirstName = profileEntity.FirstName,
                LastName = profileEntity.LastName,
                StreetName = profileEntity.StreetName,
                PostalCode = profileEntity.PostalCode,
                City = profileEntity.City,
                Role = role,
                Roles = selectRoles
            };
            return View(userProfile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserProfile userProfile)
        {
            var profileEntity = await _context.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userProfile.UserId);
            
            profileEntity.FirstName = userProfile.FirstName;
            profileEntity.LastName = userProfile.LastName;
            profileEntity.StreetName = userProfile.StreetName;
            profileEntity.PostalCode = userProfile.PostalCode;
            profileEntity.City = userProfile.City;

            await _context.SaveChangesAsync();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userProfile.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var allRoles = await _userManager.GetRolesAsync(user);
            var userRole = allRoles.FirstOrDefault();

            if(user is not null)
            {
                await _userManager.RemoveFromRoleAsync(user, userRole);
            }
            await _userManager.AddToRoleAsync(user, userProfile.Role);

            var roles = _roleManager.Roles.ToList();
            List<SelectListItem> selectRoles = new List<SelectListItem>();
            foreach(var identityRole in roles)
            {
                selectRoles.Add(new SelectListItem(identityRole.Name, identityRole.Name));
            }
            userProfile.Roles = selectRoles;

            return RedirectToAction("Users");
        }

        public async Task <IActionResult> Delete(int id) 
        {
            var profileEntity = await _context.Profiles.Include(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id); 
            await _context.SaveChangesAsync();

            await _userManager.DeleteAsync(profileEntity.User);

            return RedirectToAction("Users");
        }
    }
}
