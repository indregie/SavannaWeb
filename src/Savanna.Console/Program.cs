using Savanna.Backend;
using Savanna.Frontend;

BoardManager _boardManager = new BoardManager();
UIManager _uiManager = new UIManager(_boardManager);

Console.WriteLine("Welcome to Savanna!");

foreach (var kvp in AnimalFactory.AnimalTypes)
{
    var animalSymbol = kvp.Key;
    var animalType = kvp.Value;
    Console.WriteLine("Loaded animal " + animalType.Name);
    Console.WriteLine($"Enter {animalSymbol} to add {animalType.Name}.");
}

Thread.Sleep(2000);

while (true)
{
    try
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            char animalSymbol = char.ToUpperInvariant(key.KeyChar);
            if (AnimalFactory.AnimalTypes.ContainsKey(animalSymbol))
            {
                Type animalType = AnimalFactory.AnimalTypes[animalSymbol];
                _boardManager.AddAnimal(animalType);
            }

        }
        _boardManager.MoveAnimals();
        _uiManager.DrawBoard();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
    Thread.Sleep(1000);
}
