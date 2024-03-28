using Savanna.Backend.Actors;
using System.Reflection;

namespace Savanna.Backend;
/// <summary>
/// Factory class for creating animal objects.
/// </summary>
public static class AnimalFactory
{
    public static readonly Dictionary<char, Type> AnimalTypes = new Dictionary<char, Type>();

    static AnimalFactory()
    {
        LoadAnimalTypes();
    }

    /// <summary>
    /// Loads animal types from plugin assemblies located in plugins directory.
    /// </summary>
    private static void LoadAnimalTypes()
    {
        string pluginsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
        string[] pluginFiles = Directory.GetFiles(pluginsDirectory, "*.dll");

        foreach (string pluginFile in pluginFiles)
        {
            Assembly assembly = Assembly.LoadFrom(pluginFile);
            var types = assembly.GetTypes()
                                .Where(t => typeof(Animal).IsAssignableFrom(t) && !t.IsInterface);

            foreach (Type type in types)
            {
                var animalInstance = (Animal)Activator.CreateInstance(type)!;
                AnimalTypes[animalInstance.Symbol] = type;
            }
        }
    }

    /// <summary>
    /// Initializes an animal based on the specific animal type.
    /// </summary>
    /// <param name="animalType">Type of animal to initialize.</param>
    /// <returns>An instance of specific animal type.</returns>
    /// <exception cref="Exception">Throws an exception if provided animal type is not valid.</exception>
    public static Animal InitializeAnimal(Type animalType)
    {
        return (Animal)Activator.CreateInstance(animalType)!;
    }
}
