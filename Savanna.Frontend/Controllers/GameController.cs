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

    //invoked from view, adds animal to board and redirects to view
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

    [HttpGet]
    public IActionResult GetGameBoard()
    {
        var board = _uiManager.GetGameBoard();
        return Json(board);
    }
}
