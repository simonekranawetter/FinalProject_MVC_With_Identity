using System.ComponentModel.DataAnnotations;

namespace FinalProject_MVC_With_Identity.Models.ViewModels
{
    public class BookAMeetingViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "A name is required")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?" , ErrorMessage = "Must be a valid email address")]
        public string Email { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Must be between 2 and 250 characters", MinimumLength = 2)]
        public string Questions { get; set; }
    }
}
