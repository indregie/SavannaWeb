using Savanna.Backend.Actors;
using Savanna.Backend.Interfaces;

namespace Savanna.Backend;
/// <summary>
/// Manages the board and animals on it.
/// </summary>
public class BoardManager : IBoardManager
{
    public List<Animal> Animals { get; set; } = new List<Animal>();
    public List<Type> BirthAnimals = new List<Type>();
    private Random _random = new Random();
    public int IterationCount { get; set; } = 0;

    /// <summary>
    /// Adds animal to the board.
    /// </summary>
    /// <param name="animalType">The type of the animal to be added.</param>
    /// <exception cref="InvalidOperationException">Throws when the maximum of animals is reached.</exception>
    public void AddAnimal(Type animalType)
    {
        var possibleMoves = GetPossibleMoves();

        if (possibleMoves.Count == 0)
        {
            return;
        }

        var animal = AnimalFactory.InitializeAnimal(animalType);

        Position position = GenerateRandomPosition();
        animal.Position = position;

        bool intersects = Animals.Exists(a => a.Position.X == position.X &&
                          a.Position.Y == position.Y);

        if (!intersects)
        {
            Animals.Add(animal);
        }
        else
        {
            AddAnimal(animalType);
        }
    }

    /// <summary>
    /// Generates random position on the board.
    /// </summary>
    /// <returns>Random position</returns>
    private Position GenerateRandomPosition()
    {
        int randomX = _random.Next(0, Constants.MaxX);
        int randomY = _random.Next(0, Constants.MaxY);
        return new Position(randomX, randomY);
    }

    /// <summary>
    /// Gets a copy of animals on the board.
    /// </summary>
    /// <returns>List of animals on the board.</returns>
    public List<Animal> GetBoardAnimals()
    {
        return new List<Animal>(Animals);
    }

    /// <summary>
    /// Manages animal movement on the board: on which iteration animals of which type should move.
    /// </summary>
    public void MoveAnimals()
    {
        IterationCount++;

        foreach (var animal in Animals)
        {
            if (IterationCount % 2 == 1 && !animal.IsPredator)
            {
                animal.Move(this, _random);
            }
            if (IterationCount % 2 == 0 && animal.IsPredator)
            {
                animal.Move(this, _random);
            }
            animal.Age++;
        }

        Animals = Animals.Where(a => a.Health > 0).ToList();

        foreach (var animalType in BirthAnimals)
        {
            AddAnimal(animalType);
        }

        BirthAnimals.Clear();
    }

    /// <summary>
    /// Gets a list of all possible moves on the board.
    /// </summary>
    /// <returns>List of available positions.</returns>
    private List<Position> GetPossibleMoves()
    {
        var occupiedPositions = Animals.Select(a => a.Position).ToList();

        var allPositions = Enumerable.Range(0, Constants.MaxX)
                                     .SelectMany(x => Enumerable.Range(0, Constants.MaxY)
                                     .Select(y => new Position(x, y)))
                                     .ToList();

        var possibleMoves = allPositions.Except(occupiedPositions).ToList();

        return possibleMoves;
    }
    /// <summary>
    /// Clear animal list when calling new game.
    /// </summary>
    public void ClearAnimals()
    {
        Animals.Clear();
        BirthAnimals.Clear();
        IterationCount = 0;
    }
}
