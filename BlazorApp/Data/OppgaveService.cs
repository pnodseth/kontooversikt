using BlazorApp.Data.Models;

namespace BlazorApp.Data;

public class OppgaveService
{

    public List<Oppgave> GetTaskTemplates()
    {
        return new List<Oppgave>()
        {
            new Oppgave(title: "Støvsuge rommet", amount: 20) {Done = true},
            new Oppgave(title: "Rydde rommet", amount: 20),
            new Oppgave(title: "Gå med søppel", amount: 20),
        };
    }
}