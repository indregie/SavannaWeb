using Microsoft.EntityFrameworkCore;
using Savanna.Frontend.Data;
using Savanna.Frontend.Models;

namespace Savanna.Frontend;

public class DataService
{
    private readonly AppDbContext _dbContext;
    public DataService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveGame(string userId, string gameJson)
    {
        var existingGame = _dbContext.Games.FirstOrDefault(x => x.UserId == userId);

        if (existingGame != null)
        {
            existingGame.AnimalsJson = gameJson;
        }
        else
        {
            Game game = new Game()
            {
                UserId = userId,
                AnimalsJson = gameJson
            };

            _dbContext.Games.Add(game);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Game> LoadGame(string userId)
    {
        return await _dbContext.Games.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
