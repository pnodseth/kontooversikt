namespace BlazorApp.Data.Models;

using System.ComponentModel.DataAnnotations;

public class SingleChallenge
{
    [Key] public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int AmountOnCompletion { get; set; }
    public bool HasBeenPaid { get; set; }
    public bool IsActive { get; set; }
    public ChoreStatus Status { get; set; } = ChoreStatus.Ready;
    public Children? Child { get; set; }
}