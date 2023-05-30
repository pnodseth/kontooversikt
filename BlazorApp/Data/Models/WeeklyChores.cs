namespace BlazorApp.Data.Models;

public class WeeklyChores
{
    public int Id { get; set; }
    public int Week { get; set; }
    public int Year { get; set; }
    public Children Name { get; set; }
    public bool Completed { get; set; }
    public List<Chore> Chores { get; set; } = new();
    public bool IsPaid { get; set; }
    public int AmountPaid { get; set; }

    public WeeklyChores(int week, int year, Children name)
    {
        Week = week;
        Year = year;
        Name = name;
    }
}