using System.Drawing;

namespace Savanna.AntelopePlugin;

public class Constants
{
    private const string IconPath = "../SharedItems/Icons/antelope.png";
    private static Image icon = Image.FromFile(IconPath);   
    public const char LetterSymbol = 'A';
    public const char IconSymbol = 'i';
    public const int VisionRange = 3;
    public const bool IsPredator = false;
}
