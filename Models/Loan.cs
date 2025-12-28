using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Project.Models;

[Table("Loans", Schema = "dbo")]
public class Loan
{
    [Key]
    public int LOAN_ID { get; set; }

    public int BOOK_ID { get; set; }
    public int USER_ID { get; set; }

    public DateTime LOAN_DATE { get; set; }
    public DateTime DUE_DATE { get; set; }

    public DateTime? RETURN_DATE { get; set; }

    public bool STATUS { get; set; }

    [ForeignKey("BOOK_ID")]
    public Book Book { get; set; }

    [ForeignKey("USER_ID")]
    public User User { get; set; }

    public void CloseLoan()
    {
        RETURN_DATE = DateTime.Now;
        STATUS = false;
    }
}
