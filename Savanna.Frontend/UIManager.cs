using Savanna.Backend;
using Savanna.Backend.Actors;

namespace Savanna.Frontend;

public class UIManager
{
    private readonly BoardManager _boardManager;

    public UIManager(BoardManager boardManager)
    {
        _boardManager = boardManager;
    }

    public IEnumerable<Animal> GetBoardAnimals()
    {
        return _boardManager.GetBoardAnimals();
    }
}
