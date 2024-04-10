using Savanna.Backend.Actors;

namespace Savanna.Backend.Interfaces;

public interface IBoardManager
{
    void AddAnimal(Type animalType);
    List<Animal> GetBoardAnimals();
    void MoveAnimals();
}