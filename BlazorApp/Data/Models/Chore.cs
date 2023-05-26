using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Data.Models;

public class Chore
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public string? Child { get; set; }
    public int Amount { get; set; }
    public DateTime Created { get; set; }
    [Required]
    [Range(1,53)]
    public int WeekNumber { get; set; }
    [Required]
    [Range(2023,2030)]
    public int Year { get; set; }
    public bool Done { get; set; }
    public ChoreStatus Status { get; set; }

    public Chore(string title, int amount)
    {
        Title = title;
        Amount = amount;
        Created = DateTime.Now;
        Status = ChoreStatus.Ready;
    }
}

public enum ChoreStatus
{
    Ready,
    Pending,
    Done
}