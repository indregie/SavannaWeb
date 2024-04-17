using Microsoft.EntityFrameworkCore;
using Savanna.Frontend.Data;
using Savanna.Frontend.Interfaces;
using Savanna.Frontend.Models;

namespace Savanna.Frontend;
/// <summary>
/// Service class for managing game data in the database.
/// </summary>
public class DataService : IDataService
{
    private readonly AppDbContext _dbContext;
    public DataService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// Saves the game data to the database.
    /// </summary>
    /// <param name="userId">The ID of the logged in user.</param>
    /// <param name="gameJson">The serialized JSON string representing the game state.</param>
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
    /// Loads the game data from the database for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the logged in user.</param>
    /// <returns>The game data for the user, or null if no game is found.</returns>
    public async Task<Game?> LoadGame(string userId)
    {
        return await _dbContext.Games.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
