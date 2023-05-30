using BlazorApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    { }

    public DbSet<Chore> Chores { get; set; } = null!;
    public DbSet<WeeklyChores> WeeklyChoresEnumerable { get; set; } = null!;
    public DbSet<StreakChallenge> StreakChallenges { get; set; } = null!;
    public DbSet<StreakChallengeDate> StreakChallengesDates { get; set; } = null!;


}