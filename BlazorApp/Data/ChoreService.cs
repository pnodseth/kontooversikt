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
    }

    public async Task<List<Chore>> GetChoresThisWeek(string name)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        var calendar = cultureInfo.Calendar;

        var dateTime = DateTime.Now;
        var weekOfYear = calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        var year = DateTime.Now.Year;

        var chores = await _context.Chores.Where(c => c.Child != null && c.Child.Equals(name) && c.WeekNumber == weekOfYear && c.Year == year).ToListAsync();
        return chores;
    }

    public async Task SaveChores(List<Chore> chores)
    {
        foreach (var chore in chores)
        {
            await _context.Chores.AddAsync(chore);
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Chore>> CreateChoresThisWeek(string Name)
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

        await SaveChores(templateChores);
        return templateChores;
        
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
}