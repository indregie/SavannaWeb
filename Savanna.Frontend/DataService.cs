using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
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

    public async Task SaveGame(Game game)
    {
        _dbContext.Games.Add(game);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Game> LoadGame(int userId)
    {
        return await _dbContext.Games.FindAsync(userId);
    }
}
