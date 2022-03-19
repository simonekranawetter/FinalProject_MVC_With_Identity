using System.ComponentModel.DataAnnotations;

namespace FinalProject_MVC_With_Identity.Models.Forms
{
    public class SignUpForm
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^(([a-öA-Ö]+\s*)+)", ErrorMessage = "Must be a valid first name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage =  "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^(([a-öA-Ö]+\s*\-?)+)", ErrorMessage = "Must be a valid last name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Street Name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage =  "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^(([a-öA-Ö]+\s*)+)([\s][0-9]+)*$", ErrorMessage = "Must be a valid street name")]
        public string StreetName { get; set; } = string.Empty;

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain 5 digits")]
        [RegularExpression(@"^\d{3}(?:[ ]?\d{2})?$", ErrorMessage = "Must be a valid postal code")]
        public string PostalCode { get; set; } = string.Empty;

        [Display(Name = "City")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^(([a-öA-Ö]+\s*\-?)+)", ErrorMessage = "Must be a valid city")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?" , ErrorMessage = "Must be a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="This field is reqired")]
        [RegularExpression(@"^(?=.*?[A-Ö])(?=.*?[a-ö])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Must be a valid password.")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is reqired")]
        [Compare("Password", ErrorMessage = "Passwords not a match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = "/";
        public string RoleName { get; set; } = "user";
    }
}
