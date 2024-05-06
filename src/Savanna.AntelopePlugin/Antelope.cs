using Savanna.Backend;
using Savanna.Backend.Actors;

namespace Savanna.AntelopePlugin;
/// <summary>
/// Represents an Antelope.
/// </summary>
public class Antelope : Animal
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
        List<Position> safeMoves = new List<Position>();

        foreach (var move in potentialMoves)
        {
            bool isSafe = true;
            foreach (var animal in manager.Animals)
            {
                if (animal.IsPredator && IsWithinRange(move, animal.Position, animal.VisionRange))
                {
                    isSafe = false;
                    break;
                }
            }

            if (isSafe)
            {
                safeMoves.Add(move);
            }
        }

        Position = MoveRandomly(safeMoves, random);
        Health -= 0.5f;

        Birth(manager, typeof(Antelope));
    }

    public override byte[] GetIcon()
    {
        return System.IO.File.ReadAllBytes("C:\\Users\\indre.giedraityte\\source\\repos\\IndreGiedraityte_SavannaWeb\\src\\SharedItems\\Icons\\antelope.png");
    }
}