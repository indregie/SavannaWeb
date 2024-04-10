using Savanna.Backend;
using Savanna.Backend.Actors;
using Savanna.Backend.Interfaces;
using Savanna.Frontend.Interfaces;

namespace Savanna.Frontend;

public class UIManager : IUIManager
{
    private readonly IBoardManager _boardManager;

    public UIManager(IBoardManager boardManager)
    {
        _boardManager = boardManager;
    }

    public List<List<char>> GetGameBoard()
    {
        List<Animal> animals = _boardManager.GetBoardAnimals();
        List<List<char>> boardList = new List<List<char>>();

        for (int i = 0; i < Constants.MaxY; i++)
        {
            List<char> row = new List<char>();
            for (int j = 0; j < Constants.MaxX; j++)
            {
                row.Add('-');
            }
            boardList.Add(row);
        }

        foreach (var animal in animals)
        {
            boardList[animal.Position.Y][animal.Position.X] = animal.Symbol;
        }

        return boardList;
    }
}
