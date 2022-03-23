using System.ComponentModel.DataAnnotations;

namespace FinalProject_MVC_With_Identity.Models
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; }

        [Display(Name = "Upload File")]
        public IFormFile File { get; set; }
    }
}
