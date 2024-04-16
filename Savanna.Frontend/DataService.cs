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
    /// <summary>
    /// Saves game to the database
    /// </summary>
    /// <param name="userId">Logged in user</param>
    /// <param name="gameJson">Serialized List<Animal></param>
    /// <returns></returns>
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
    /// <summary>
    /// Loads game from database
    /// </summary>
    /// <param name="userId">Logged in user</param>
    /// <returns>Game by the user</returns>
    public async Task<Game?> LoadGame(string userId)
    {
        return await _dbContext.Games.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
