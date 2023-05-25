namespace BlazorApp.Data.Models;

public class Oppgave
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public Person? Person { get; set; }
    public int Amount { get; set; }
    public DateTime Created { get; set; }
    public bool Done { get; set; }

    public Oppgave(string title, int amount)
    {
        Title = title;
        Amount = amount;
        Created = DateTime.Now;
    }
}