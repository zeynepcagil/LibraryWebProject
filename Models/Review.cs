using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Project.Models;

public class Review
{
    [Key]
    public int REVIEW_ID { get; set; }

    public int BOOK_ID { get; set; }
    [ForeignKey("BOOK_ID")]
    public Book Book { get; set; }

    public int USER_ID { get; set; }
    [ForeignKey("USER_ID")]
    public User User { get; set; }

    public string COMMENT { get; set; }
    public int RATING { get; set; } // 1 to 10
    public DateTime REVIEW_DATE { get; set; }
}