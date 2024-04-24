using Savanna.Backend;
using Savanna.Backend.Actors;
using Savanna.Backend.Interfaces;
using Savanna.Frontend.Interfaces;

namespace Savanna.Frontend;

public class DrawingService : IDrawingService
{
    private readonly IBoardManager _boardManager;

    public DrawingService(IBoardManager boardManager)
    {
        _boardManager = boardManager;
    }

    public List<List<long?>> GetGameBoard()
    {
        List<Animal> animals = _boardManager.GetBoardAnimals();
        List<List<long?>> boardList = new List<List<long?>>();

        for (int i = 0; i < Constants.MaxY; i++)
        {
            List<long?> row = new List<long?>();
            for (int j = 0; j < Constants.MaxX; j++)
            {
                row.Add(null); ;
            }
            boardList.Add(row);
        }

        foreach (var animal in animals)
        {
            boardList[animal.Position.Y][animal.Position.X] = animal.Id;
        }

        return boardList;
    }
}
