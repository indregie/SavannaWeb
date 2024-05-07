using Savanna.Backend;
using Savanna.Backend.Actors;

namespace Savanna.LionPlugin;

/// <summary>
/// Represents a Lion.
/// </summary>
public class Lion : Animal
{
    public override char Symbol { get; } = Constants.Symbol;
    public override int VisionRange { get; } = Constants.VisionRange;
    public override bool IsPredator { get; set; } = Constants.IsPredator;
    public override Dictionary<long, int> SurroundingAnimals { get; set; } = new Dictionary<long, int>();

    /// <summary>
    /// Moves the animal according to the game rules.
    /// </summary>
    /// <param name="manager">.</param>
    /// <param name="random">Random number generator.</param>
    public override void Move(BoardManager manager, Random random)
    {
        UpdateSurroundingAnimals(manager.Animals);

        List<Position> potentialMoves = GetPotentialMoves(Position, Backend.Constants.MaxX, Backend.Constants.MaxY);
        List<Position> huntMoves = new List<Position>();

        foreach (var move in potentialMoves)
        {
            bool isToHunt = false;
            foreach (var animal in manager.Animals)
            {
                if (animal.IsPredator == false && IsWithinRange(move, animal.Position, animal.VisionRange))
                {
                    isToHunt = true;
                    break;
                }
            }

            if (isToHunt)
            {
                huntMoves.Add(move);
            }
        }

        if (huntMoves.Count > 0)
        {
            Position = MoveRandomly(huntMoves, random);
        } else
        {
            Position = MoveRandomly(potentialMoves, random);
        }
        
        Health -= 0.5f;

        Birth(manager, typeof(Lion));
    }

    /// <summary>
    /// Loads icon from Icons folder
    /// </summary>
    /// <returns>Array of bytes displaying an animal icon</returns>
    public override byte[] GetIcon()
    {
        return System.IO.File.ReadAllBytes(@"..\\SharedItems\\Icons\\lion.png");
    }
}

