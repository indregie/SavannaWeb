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
        Game game = new Game() 
        { 
            UserId = userId,
            AnimalsJson = gameJson
        };

        _dbContext.Games.Add(game);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Game> LoadGame(string userId, string game)
    {
        return await _dbContext.Games.FindAsync(userId);
    }
}
