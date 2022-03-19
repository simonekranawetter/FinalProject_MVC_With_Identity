using FinalProject_MVC_With_Identity.Models.Forms;
using FinalProject_MVC_With_Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_MVC_With_Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IProfileManager _profileManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IProfileManager profileManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _profileManager = profileManager;
        }
        #region SignUp
        [Route("signup")]
        [HttpGet]
        public IActionResult SignUp(string returnUrl= null)
        {
            var form = new SignUpForm();
            if(returnUrl != null)
            {
                form.ReturnUrl = returnUrl;
            }
            return View();
        }
        [Route("signup")]
        [HttpPost]
        public IActionResult SignUp(SignUpForm form)
        {
            return View();
        }
        #endregion
    }
}
