using Savanna.Backend;
using Savanna.Backend.Actors;
using Savanna.Backend.Interfaces;

namespace Savanna.ConsoleApp;

/// <summary>
/// Manages the user interface for displaying the game board.
/// </summary>
public class UIManager
{
    private IBoardManager _boardManager;
    public UIManager(IBoardManager boardManager)
    {
        _boardManager = boardManager;
    }

    /// <summary>
    /// Draws the game board with the current state of animals.
    /// </summary>
    public void DrawBoard()
    {
        List<Animal> animals = _boardManager.GetBoardAnimals();

        char[,] board = new char[Constants.MaxY, Constants.MaxX];

        for (int i = 0; i < Constants.MaxY; i++)
        {
            for (int j = 0; j < Constants.MaxX; j++)
            {
                board[i, j] = '-';
            }
        }

        foreach (var animal in animals)
        {
            try
            {
                board[animal.Position.Y, animal.Position.X] = animal.Symbol;
            }
            catch
            {
                Console.WriteLine(animal);
            }
        }

        Console.Clear();

        for (int i = 0; i < Constants.MaxY; i++)
        {
            for (int j = 0; j < Constants.MaxX; j++)
            {
                Console.Write(board[i, j]);
            }
            Console.WriteLine();
        }
    }
}

