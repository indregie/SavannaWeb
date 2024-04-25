using Savanna.Backend.Actors;

namespace Savanna.Backend.Interfaces;

public interface IBoardManager
{
    List<Animal> Animals { get; }
    int IterationCount { get; }
    void AddAnimal(Type animalType);
    List<Animal> GetBoardAnimals();
    void MoveAnimals();
    void ClearAnimals();
}
