using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Project.Models
{
    public class Review
    {
        [Key]
        public int REVIEW_ID { get; set; }

        public int BOOK_ID { get; set; }
        public int USER_ID { get; set; }

        [Range(1, 10)]
        public int RATING { get; set; } // 1-10 Puan

        public string? COMMENT { get; set; } // Yorum

        public DateTime REVIEW_DATE { get; set; } = DateTime.Now;

        // Ýliþkiler
        [ForeignKey("BOOK_ID")]
        public virtual Book? Book { get; set; }

        [ForeignKey("USER_ID")]
        public virtual User? User { get; set; }
    }
}