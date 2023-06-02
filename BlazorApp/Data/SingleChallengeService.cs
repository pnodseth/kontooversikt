using BlazorApp.Data.Models;
using BlazorApp.Store;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class SingleChallengeService
{
    private readonly MyDbContext _context;
    private readonly AccountsService _accountsService;
    private readonly AppState _appState;

    public SingleChallengeService(MyDbContext context, AccountsService accountsService, AppState appState)
    {
        _context = context;
        _accountsService = accountsService;
        _appState = appState;
    }

    public async Task<SingleChallenge?> GetActiveChallenge(Children child)
    {
        var activeChallenge = await _context.SingleChallenges.FirstOrDefaultAsync(c => c.Child == child && c.IsActive);

        return activeChallenge;
    }

    public Task<List<SingleChallenge>> GetStreakChallengeTemplates()
    {
        return Task.FromResult(new List<SingleChallenge>()
        {
            new()
            {
                Title = "Støvsuge stua",
                AmountOnCompletion = 20,
            },
            new()
            {
                Title = "Rydde og støvsuge rommet",
                AmountOnCompletion = 20
            },
            new()
            {
                Title = "Lekser",
                AmountOnCompletion = 30
            },
        });
    }

    public async Task CreateSingleChallenge(SingleChallenge challenge)
    {
        await _context.SingleChallenges.AddAsync(challenge);
        await _context.SaveChangesAsync();
    }

    public async Task Payout(int id)
    {
        var challenge = await _context.SingleChallenges.FindAsync(id);
        if (challenge is null)
        {
            throw new Exception($"Could not find challenge with id {id}");
        }

        await _accountsService.TransferFromMainAccount(_appState.CurrentChild, challenge.AmountOnCompletion,
            $"SingleChallenge fullfort");
        challenge.IsActive = false;
        challenge.HasBeenPaid = true;
        await _context.SaveChangesAsync();
    }

    public async Task SetStatus(int id, ChoreStatus status)
    {
        var challenge = await _context.SingleChallenges.FindAsync(id);
        if (challenge is null)
        {
            throw new Exception($"Could not find challenge with id {id}");
        }

        if (status == ChoreStatus.Rejected)
        {
            _context.SingleChallenges.Remove(challenge);
        }
        else
        {
            challenge.Status = status;
        }

        await _context.SaveChangesAsync();
    }
}