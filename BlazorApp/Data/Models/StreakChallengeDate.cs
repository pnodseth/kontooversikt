using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Data.Models;

public class StreakChallengeDate
{
    [Key] public int Id { get; set; }

    public DateTime DateCompleted { get; set; }
    public ChoreStatus Status { get; set; } = ChoreStatus.Ready;

    [ForeignKey("StreakChallengeId")] public StreakChallenge StreakChallenge { get; set; } = null!;
    public int StreakChallengeId { get; set; }
}