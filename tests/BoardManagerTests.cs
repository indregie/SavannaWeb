using Savanna.AntelopePlugin;
using Savanna.Backend;
using Savanna.Backend.Actors;
using Savanna.Backend.Interfaces;
using Savanna.LionPlugin;

namespace Savanna.Tests;

public class BoardManagerTests
{
    private readonly IBoardManager _boardManager;

    public BoardManagerTests()
    {
        _boardManager = new BoardManager();
    }

    [Fact]
    public void AddAnimal_AddsAnimalToBoard()
    {
        //Act
        _boardManager.AddAnimal(typeof(Antelope));

        //Assert
        Assert.Single(_boardManager.Animals);
    }

    [Fact]
    public void MoveAnimals_MovesAnimalsOnBoard()
    {
        //Arrange
        _boardManager.AddAnimal(typeof(Antelope));

        //Act
        var initialPosition = _boardManager.Animals[0].Position;

        for (int i = 0; i < 10; i++)
        {
            _boardManager.MoveAnimals();
        }

        //Assert
        var finalPosition = _boardManager.Animals[0].Position;
        Assert.NotEqual(initialPosition, finalPosition);
    }

    [Fact]
    public void GetBoardAnimals_ReturnsCopyofAnimalsList()
    {
        //Arrange
        _boardManager.AddAnimal(typeof(Antelope));

        //Act
        var animals = _boardManager.GetBoardAnimals();

        //Assert
        Assert.NotSame(_boardManager.Animals, animals);
    }

    [Fact]
    public void Birth_AddsNewAnimalToBirthListWhenConditionMet()
    {
        //Arrange
        var lion1 = new Lion();
        lion1.Position = new Position(0, 0);
        _boardManager.Animals.Add(lion1);

        var lion2 = new Lion();
        lion2.Position = new Position(1, 0);
        _boardManager.Animals.Add(lion2);

        //Act
        for (int i = 0; i < 6; i++)
        {
            _boardManager.MoveAnimals();
            lion1.Position = new Position(0, 0);
            lion2.Position = new Position(1, 0);
        }

        //Assert
        Assert.Equal(4, _boardManager.Animals.Count);
    }

    [Fact]
    public void Death_RemovesAnimalFromBoardAfterCertainMoves()
    {
        //Arrange
        var antelope = new Antelope();
        antelope.Position = new Position(0, 0);
        _boardManager.Animals.Add(antelope);

        //Act
        while (antelope.Health > 0)
        {
            _boardManager.MoveAnimals();
        }

        //Assert
        Assert.Empty(_boardManager.Animals);
    }
}
