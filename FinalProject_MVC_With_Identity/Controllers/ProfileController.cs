using FinalProject_MVC_With_Identity.Models;
using FinalProject_MVC_With_Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_MVC_With_Identity.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileManager _profileManager;
        private readonly IWebHostEnvironment _host;

        public ProfileController(IProfileManager profileManager, IWebHostEnvironment host)
        {
            _profileManager = profileManager;
            _host = host;
        }

        [HttpGet("{id}")]
        //[Route("profile/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var profile = await _profileManager.ReadAsync(id);
            return View(profile);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Index(UserProfile userProfile)
        {
            var id = userProfile.Id;
            var profile = await _profileManager.ReadAsync(id);

            if (userProfile.File is not null) // or if it doesn't change. Fix meeeeeee!
            {
                string wwwrootPath = _host.WebRootPath;
                string fileName = $"{Path.GetFileNameWithoutExtension(userProfile.File.FileName)}_{Guid.NewGuid()}{Path.GetExtension(userProfile.File.FileName)}";
                string imageurl = $"images/users/{fileName}";
                string filePath = Path.Combine($"{wwwrootPath}/images/users", fileName);

                if (profile.ProfileImageUrl is not null)
                {
                    string oldProfileImagePath = Path.Combine(wwwrootPath, profile.ProfileImageUrl);

                    if (System.IO.File.Exists(oldProfileImagePath))
                    {
                        System.IO.File.Delete(oldProfileImagePath);
                    }

                }
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await userProfile.File.CopyToAsync(fs);
                }
                userProfile.ProfileImageUrl = imageurl;
                profile.ProfileImageUrl = imageurl;
            }       
               
            await _profileManager.UpdateAsync(userProfile);
            return RedirectToAction("Index", userProfile.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (UserProfile userprofile)
        {
            await _profileManager.UpdateAsync(userprofile);
            return RedirectToAction("Index", userprofile.Id);
        }
    }
}
