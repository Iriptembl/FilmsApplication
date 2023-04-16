using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.ViewModel;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Birth year")]
    public int Year { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm password")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}