using Microsoft.AspNetCore.Mvc;
using Savanna.Backend;

namespace Savanna.Frontend.Controllers;

public class GameController : Controller
{
    private readonly BoardManager _boardManager;
    private readonly UIManager _uiManager;

    public GameController()
    {
        _boardManager = new BoardManager();
        _uiManager = new UIManager(_boardManager);
    }
    public IActionResult Index()
    {
        ViewData["UIManager"] = _uiManager;
        return View();
    }

    [HttpPost]
    public IActionResult HandleInput(char animalSymbol)
    {
        if (AnimalFactory.AnimalTypes.ContainsKey(animalSymbol))
        {
            Type animalType = AnimalFactory.AnimalTypes[animalSymbol];
            _boardManager.AddAnimal(animalType);
        }

        return RedirectToAction("Index");   
    }
}
