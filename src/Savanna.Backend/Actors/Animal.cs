namespace Savanna.Backend.Actors;
/// <summary>
/// Base class for animals in the Savanna game.
/// </summary>
public abstract class Animal
{
    public long Id { get; set; }
    public static long NextId { get; set; }
    public Position Position { get; set; }
    public abstract int VisionRange { get; }
    public float Health { get; set; } = 10;
    public abstract Dictionary<long, int> SurroundingAnimals { get; set; }
    public abstract bool IsPredator { get; set; }
    public int Age { get; set; } = 0;
    public int Offsprings { get; set; } = 0;
    public virtual char Icon => '?';
    public virtual char Letter => '?';
    public virtual char Symbol => Letter;

    protected Animal()
    {
        Id = NextId;
        NextId++;
    }

    /// <summary>
    /// Calculates the potential movements for the animal based on its current position.
    /// </summary>
    /// <param name="currentPosition"> Current position of an animal.</param>
    /// <param name="maxX">The maximum X coordinate.</param>
    /// <param name="maxY">The maximum Y coordinate.</param>
    /// <returns>List of potential moves for an animal.</returns>
    protected List<Position> GetPotentialMoves(Position currentPosition, int maxX, int maxY)
    {
        int[] offsetX = { -1, 0, 1, 1, 1, 0, -1, -1 };
        int[] offsetY = { 1, 1, 1, 0, -1, -1, -1, 0 };

        List<Position> potentialMoves = new List<Position>();

        for (int k = 0; k < offsetX.Length; k++)
        {
            int x = currentPosition.X + offsetX[k];
            int y = currentPosition.Y + offsetY[k];

            if (x >= 0 && y >= 0 && x < maxX && y < maxY)
            {
                potentialMoves.Add(new Position(x, y));
            }
        }

        return potentialMoves;
    }

    /// <summary>
    /// Checks idf the distance between 2 positions is within specified range.
    /// </summary>
    /// <param name="position1">The first position.</param>
    /// <param name="position2">The second position.</param>
    /// <param name="range">Animal vision range</param>
    /// <returns>True if the distance is within the range, false if it does not.</returns>
    protected bool IsWithinRange(Position position1, Position position2, int range)
    {
        int distance = Math.Max(Math.Abs(position1.X - position2.X), Math.Abs(position1.Y - position2.Y));
        return distance <= range;
    }

    /// <summary>
    /// Moves the animal randomly among provided potential moves.
    /// </summary>
    /// <param name="potentialMoves">List of potential moves.</param>
    /// <param name="random">Random number generator.</param>
    /// <returns>A new position for an animal.</returns>
    protected Position MoveRandomly(List<Position> potentialMoves, Random random)
    {
        if (potentialMoves.Count > 0)
        {
            int index = random.Next(potentialMoves.Count);
            return potentialMoves[index];
        }
        return Position;
    }

    /// <summary>
    /// Counts iterations for surrounding animals.
    /// </summary>
    /// <param name="animals">List of surrounding animals</param>
    protected void UpdateSurroundingAnimals(List<Animal> animals)
    {
        Dictionary<long, int> surroundingAnimals = new Dictionary<long, int>();
        foreach (var animal in animals)
        {
            if (GetType() == animal.GetType() && animal.Id != this.Id && IsWithinRange(Position, animal.Position, VisionRange))
            {
                var iterationsWithinRange = SurroundingAnimals.GetValueOrDefault(animal.Id, 0);
                surroundingAnimals[animal.Id] = iterationsWithinRange + 1;
            }
        }
        SurroundingAnimals = surroundingAnimals;
    }

    /// <summary>
    /// Moves the animal according to the game rules.
    /// </summary>
    /// <param name="animals">The list of animals on the game board.</param>
    /// <param name="random">Random number generator.</param>
    public abstract void Move(BoardManager manager, Random random);

    /// <summary>
    /// Handles the birth of new animal if the number of surrounding animals of the same type is 3 or more.
    /// </summary>
    /// <param name="manager">The board manager managing the game board.</param>
    /// <param name="animalType">Type of nimal to be added to the birth list.</param>
    protected void Birth(BoardManager manager, Type animalType)
    {
        List<KeyValuePair<long, int>> birthAnimals = SurroundingAnimals.Where(kvp => kvp.Value >= 3).ToList();

        foreach (var kvp in birthAnimals)
        {
            SurroundingAnimals.Remove(kvp.Key);
            manager.BirthAnimals.Add(animalType);
            this.Offsprings++;
        }
    }
}