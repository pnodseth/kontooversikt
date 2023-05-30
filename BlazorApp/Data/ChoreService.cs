using System.Globalization;
using BlazorApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class ChoreService
{
    private readonly MyDbContext _context;

    public ChoreService(MyDbContext context)
    {
        _context = context;
    }

    public List<Chore> GetTemplateChores()
    {
        return new List<Chore>()
        {
            new Chore(title: "Støvsuge rommet", amount: 20),
            new Chore(title: "Rydde rommet", amount: 20),
            new Chore(title: "Gå med søppel", amount: 20),
        };

        var klaraChores = new List<Chore>()
        {
            new Chore(title: "Brette klær meRd mamma", amount: 20),
            new Chore(title: "Vanne planter Rute", amount: 20),
            new Chore(title: "Rydde rommet", amount: 20),
            new Chore(title: "Vaske badet", amount: 20),
        };
    }

    public async Task<WeeklyChores?> GetChoresThisWeek(Children child)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        var calendar = cultureInfo.Calendar;

        var dateTime = DateTime.Now;
        var weekOfYear = calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        var year = DateTime.Now.Year;

        // var chores = await _context.Chores.Where(c => c.Child != null && c.Child.Equals(name) && c.WeekNumber == weekOfYear && c.Year == year).ToListAsync();
        var weeklyChores =
            await _context.WeeklyChoresEnumerable.Include(e => e.Chores)
                .FirstOrDefaultAsync(c => c.Name == child && c.Week == weekOfYear && c.Year == year);

        return weeklyChores;
    }


    public async Task SaveWeeklyChores(WeeklyChores weeklyChores)
    {
        await _context.WeeklyChoresEnumerable.AddAsync(weeklyChores);
        await _context.SaveChangesAsync();
    }

    public async Task<WeeklyChores> CreateWeeklyChores(string Name)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        var calendar = cultureInfo.Calendar;
        var dateTime = DateTime.Now;
        var weekOfYear = calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        var year = DateTime.Now.Year;

        var templateChores = GetTemplateChores();
        foreach (var templateChore in templateChores)
        {
            templateChore.Child = Name;
            templateChore.WeekNumber = weekOfYear;
            templateChore.Year = year;
        }

        var weeklyChores = new WeeklyChores(weekOfYear, year, Children.Imre);
        weeklyChores.Chores = templateChores;
        await SaveWeeklyChores(weeklyChores);
        // await SaveChores(templateChores);
        return weeklyChores;
    }

    public async Task SetChoreStatus(int id, ChoreStatus status)
    {
        var chore = await _context.Chores.FindAsync(id);
        if (chore is null)
        {
            return;
        }

        chore.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task SetWeeklyChoresDone(int weeklyChoresId)
    {
        var weeklyChores = await _context.WeeklyChoresEnumerable.FindAsync(weeklyChoresId);
        if (weeklyChores != null)
        {
            weeklyChores.Completed = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task SetWeeklyChoresPaid(int id, int amount)
    {
        var weeklyChores = await _context.WeeklyChoresEnumerable.FindAsync(id);
        if (weeklyChores is null) return;
        weeklyChores.IsPaid = true;
        weeklyChores.AmountPaid = amount;

        await _context.SaveChangesAsync();
    }

    
    
    
}