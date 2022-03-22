using FinalProject_MVC_With_Identity.Models;
using FinalProject_MVC_With_Identity.Models.ViewModels;
using FinalProject_MVC_With_Identity.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalProject_MVC_With_Identity.Controllers
{
    public class HomeController : Controller
    {   
        private readonly IMailService _mailService;
        public HomeController(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(BookAMeetingViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("bookameeting@omivues.com", $"From: {model.Name} - {model.Email}, Message: {model.Questions}");
                ViewBag.UserMessage = "Mail Sent";
            }
            return View();
        }

        [HttpGet("services")]
        public IActionResult Services()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("contactform@omivues.com", $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}