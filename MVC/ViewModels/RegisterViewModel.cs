using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email address")]
    public string EmailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Confirm password is required")]
    [Display(Name = "Confirm password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}