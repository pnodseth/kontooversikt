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
    
    public int GetCurrentStreak()
    {
        var validDates = DatesCompleted.Where(x => x.Status == ChoreStatus.Done || x.Status == ChoreStatus.Pending).ToList();
        if (!validDates.Any())
        {
            return 0;
        }

        validDates.Sort((a, b) => b.DateCompleted.CompareTo(a.DateCompleted));

        int currentStreak = 1;  // Start from 1 as we always count the current day in the streak

        for (var i = 1; i < validDates.Count; i++)
        {
            if ((validDates[i-1].DateCompleted.Date - validDates[i].DateCompleted.Date).Days == 1)
            {
                if (validDates[i].Status == ChoreStatus.Rejected)
                {
                    break;
                }
                currentStreak++;
            }
            else
            {
                break;
            }
        }

        return currentStreak;
    }
}