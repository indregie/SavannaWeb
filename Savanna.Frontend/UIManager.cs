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

    public char[,] GetGameBoard()
    {
        List<Animal> animals = _boardManager.GetBoardAnimals();
        char[,] board = new char[Constants.MaxY, Constants.MaxX];

        for (int i = 0; i < Constants.MaxY;  i++)
        {
            for (int j = 0; j < Constants.MaxX; j++)
            {
                board[i, j] = '-';
            }
        }

        foreach (var animal in animals)
        {
            board[animal.Position.Y, animal.Position.X] = animal.Symbol;
        }

        return board;
    }
}
