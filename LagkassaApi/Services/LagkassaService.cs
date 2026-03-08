using LagkassaApi.DTOs;
using LagkassaApi.Models;

namespace LagkassaApi.Services;

public class LagkassaService
{
    private readonly List<Player> _players = new();
    private readonly List<Activity> _activities = new();

    private int _nextPlayerId = 1;
    private int _nextActivityId = 1;

    public LagkassaService()
    {
        SeedPlayers();
    }

    private void SeedPlayers()
    {
        var names = new List<string>
        {
            "Ali", "Omar", "Sara", "Erik", "Johan",
            "Maja", "Lina", "Adam", "Noah", "Elsa",
            "Hugo", "Emma", "Nora", "Leo", "Lucas",
            "Elin", "William", "Ella", "Anton", "Ibrahim"
        };

        foreach (var name in names)
        {
            _players.Add(new Player
            {
                Id = _nextPlayerId++,
                Name = name,
                Balance = 500
            });
        }
    }

    public List<Player> GetAllPlayers()
    {
        return _players;
    }

    public Player? GetPlayerById(int id)
    {
        return _players.FirstOrDefault(p => p.Id == id);
    }

    public bool Deposit(int playerId, decimal amount)
    {
        var player = GetPlayerById(playerId);

        if (player == null || amount <= 0)
            return false;

        player.Balance += amount;
        return true;
    }

    public List<Activity> GetAllActivities()
    {
        return _activities;
    }

    public object? CreateActivity(CreateActivityDto dto)
    {
        if (dto.CalledPlayerIds.Count == 0)
            return null;

        var calledPlayers = _players
            .Where(p => dto.CalledPlayerIds.Contains(p.Id))
            .ToList();

        if (calledPlayers.Count != dto.CalledPlayerIds.Count)
            return null;

        var costPerPlayer = dto.TotalCost / calledPlayers.Count;

        foreach (var player in calledPlayers)
        {
            player.Balance -= costPerPlayer;
        }

        var activity = new Activity
        {
            Id = _nextActivityId++,
            Name = dto.Name,
            Date = dto.Date,
            TotalCost = dto.TotalCost,
            CalledPlayerIds = dto.CalledPlayerIds,
            CostPerPlayer = costPerPlayer
        };

        _activities.Add(activity);

        return new
        {
            activity.Id,
            activity.Name,
            activity.Date,
            activity.TotalCost,
            activity.CostPerPlayer
        };
    }
}
