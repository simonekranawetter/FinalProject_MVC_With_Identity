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
                Id = id,
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
        //FIX ME!
        [HttpPost]
        public async Task<IActionResult> Edit(UserProfile userProfile)
        {
            var profileEntity = await _context.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userProfile.Id);
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == userProfile.Role);
            var editedUserProfile = new UserProfile
            {
                Id = profileEntity.Id.ToString(),
                FirstName = profileEntity.FirstName,
                LastName = profileEntity.LastName,
                StreetName = profileEntity.StreetName,
                PostalCode = profileEntity.PostalCode,
                City = profileEntity.City,
                Role = role.ToString(),
            };

            await _context.SaveChangesAsync();
            return View();
        }
        //Fix meeeeee!
        public async Task <IActionResult> Delete(string id)
        {
            var profileEntity = await _context.Profiles.FindAsync(id);
            _context.Profiles.Remove(profileEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }
    }
}
