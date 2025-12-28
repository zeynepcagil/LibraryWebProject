using System.ComponentModel.DataAnnotations;

namespace Library_Project.Models;

public class RegisterViewModel
{
    [Required]
    public string USERNAME { get; set; }

    [Required, EmailAddress]
    public string EMAIL { get; set; }

    [Required]
    public string PASSWORD { get; set; }

    [Required]
    public string NAME { get; set; }

    [Required]
    public string SURNAME { get; set; }

    [Required]
    public DateTime BIRTHDATE { get; set; }

    [Required]
    public string PHONE { get; set; }
}
