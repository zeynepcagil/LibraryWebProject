namespace Library_Project.Models;

public class MyBookViewModel
{
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int RemainingDays { get; set; }
}
