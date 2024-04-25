using Savanna.Frontend.Models;

namespace Savanna.Frontend.Interfaces
{
    public interface IDataService
    {
        Task<Game?> LoadGame(string userId);
        Task SaveGame(string userId, string gameJson);
    }
}