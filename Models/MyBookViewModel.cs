using System;

namespace Library_Project.Models
{
    public class MyBookViewModel
    {
        // Kitap ID'si (Detay sayfasına gitmek için lazım)
        public int BookId { get; set; }

        // Kitap Başlığı
        public string Title { get; set; } = string.Empty;

        // Son Teslim Tarihi
        public DateTime DueDate { get; set; }

        // Kalan Gün Sayısı
        public int RemainingDays { get; set; }

        // Ceza Miktarı 
        public double FineAmount { get; set; }
    }
}