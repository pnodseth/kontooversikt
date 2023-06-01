using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Data.Models;

public class StreakChallenge
{
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int StreakNumberForCompleted { get; set; }
    public bool IsActive { get; set; }
    public DateOnly? StartsAt { get; set; }
    public DateOnly? EndsAt { get; set; }
    public bool HasAchievedTargetStreak { get; set; }
    public List<StreakChallengeDate> DatesCompleted { get; set; } = new();
    public int AmountOnCompletion { get; set; }
    public bool HasBeenPaid { get; set; }
    public Children? Child { get; set; }
    
    public List<ChoreStatus> GetCurrentStreak()
    {
        var validDates = DatesCompleted.Where(x => x.Status == ChoreStatus.Done || x.Status == ChoreStatus.Pending).ToList();
        if (!validDates.Any())
        {
            return new List<ChoreStatus>();
        }

        validDates.Sort((a, b) => b.DateCompleted.CompareTo(a.DateCompleted));

        List<ChoreStatus> currentStreak = new List<ChoreStatus>();  // Start from an empty list

        for (var i = 0; i < validDates.Count; i++)
        {
            if (i > 0 && (validDates[i-1].DateCompleted.Date - validDates[i].DateCompleted.Date).Days != 1)
            {
                break;
            }
            if (validDates[i].Status == ChoreStatus.Rejected)
            {
                break;
            }
            currentStreak.Add(validDates[i].Status);

            // If the streak has reached the target length, stop adding
            if (currentStreak.Count >= StreakNumberForCompleted)
            {
                break;
            }
        }

        currentStreak.Reverse(); // Reverse the list to start with the first day of the current streak
        return currentStreak;
    }   
}