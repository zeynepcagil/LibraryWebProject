using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Project.Models;

[Table("Users", Schema = "dbo")]
public class User
{
    [Key]
    public int USER_ID { get; set; }

    [Required]
    public string USERNAME { get; set; } = string.Empty;

    [Required]
    public string EMAIL { get; set; } = string.Empty;

    [Required]
    public string PASSWORD { get; set; } = string.Empty;

    public string NAME { get; set; } = string.Empty;
    public string SURNAME { get; set; } = string.Empty;

    public DateTime BIRTHDATE { get; set; }

    public bool RANK { get; set; }

    public string PHONE { get; set; } = string.Empty;

    public ICollection<Loan> Loans { get; set; }


    public bool CanLoan(int maxLoanCount = 3)
    {
        return Loans == null || Loans.Count(l => l.STATUS) < 3;
    }

}