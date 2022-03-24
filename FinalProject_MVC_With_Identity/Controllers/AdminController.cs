using FinalProject_MVC_With_Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_MVC_With_Identity.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            return View(await _context.Profiles.Include(x => x.User).ToListAsync());
        }
        
        //[HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var profileEntity = await _context.Profiles.FirstOrDefaultAsync(p => p.Id.ToString() == id);//.FindAsync(id);
            //if (profileEntity == null)
            //{
            //    return NotFound();
            //}
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", profileEntity.UserId);
            //
            //return View(profileEntity);
            return View();
        }

        public async Task <IActionResult> Delete(string id)
        {
            var profileEntity = await _context.Profiles.FindAsync(id);
            _context.Profiles.Remove(profileEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }
    }
}
