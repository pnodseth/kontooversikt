using BlazorApp.Data.Models;
using BlazorApp.Store;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class StreakChallengeService
{
    private readonly MyDbContext _context;
    private readonly AccountsService _accountsService;
    private readonly AppState _appState;

    public StreakChallengeService(MyDbContext context, AccountsService accountsService, AppState appState)
    {
        _context = context;
        _accountsService = accountsService;
        _appState = appState;
    }

    public async Task<List<StreakChallenge>> GetActiveChallenges(Children child)
    {
        var activeChallenges = await _context.StreakChallenges.Include(c => c.DatesCompleted).Where(c => c.Child == child && c.IsActive && !c.HasBeenPaid).ToListAsync();
        return activeChallenges;

    }

    public async Task<List<StreakChallenge>> GetStreakChallengeTemplates()
    {
        return new List<StreakChallenge>()
        {
            new StreakChallenge()
            {
                Title = "Morgenrutine",
                Description =
                    "På morgenen når det er skole skal TV/Ipad/Switch skrus av senest kl 7:30, og da skal du kle på deg og pusse tennene",
                StreakNumberForCompleted = 5,
                AmountOnCompletion = 50,

            },
            new StreakChallenge()
            {
                Title = "Kveldsrutine",
                Description = "Skru av TV/IPAD/SWTICH senest kl 19, da er skjermtid over, og det er tid for kveldsmat, og gjerne lese bok, eller spille kort eller brettspill",
                StreakNumberForCompleted = 5,
                AmountOnCompletion = 50
            },
            new StreakChallenge()
            {
                
                Title = "Lekser",
                Description = "Gjøre lekser",
                StreakNumberForCompleted = 5,
                AmountOnCompletion = 30
            },
        };

    }
       public async Task CreateStreakChallenge(StreakChallenge challenge)
       {
           await _context.StreakChallenges.AddAsync(challenge);
           await _context.SaveChangesAsync();
       }

       public async Task AddStreakDate(int  challengeId)
       {
           var date = new StreakChallengeDate()
           {
               Status = ChoreStatus.Pending,
               DateCompleted = DateTime.Today
           };
           
           var challenge = await _context.StreakChallenges.Include(x => x.DatesCompleted).FirstOrDefaultAsync(x => x.Id == challengeId);
           if (challenge is null)
           {
               Console.WriteLine($"Could not find challenge with Id: {challengeId}");
               return;
           }
           challenge.DatesCompleted.Add(date);
           
           if (challenge.GetCurrentStreak() >= challenge.StreakNumberForCompleted)
           {
               challenge.HasAchievedTargetStreak = true;
           }
           
           await _context.SaveChangesAsync();
       }

       public async Task Payout(int id)
       {
           var challenge = await _context.StreakChallenges.FindAsync(id);
           if (challenge is null)
           {
               throw new Exception($"Could not find challenge with id {id}");
           }

           await _accountsService.TransferFromMainAccount(_appState.CurrentChild, challenge.AmountOnCompletion, $"StreakChallenge fullfort");
           challenge.IsActive = false;
           challenge.HasBeenPaid = true;
           await _context.SaveChangesAsync();
       }
}