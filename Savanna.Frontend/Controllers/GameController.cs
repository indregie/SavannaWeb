using Microsoft.AspNetCore.Mvc;
using Savanna.Backend;
using Savanna.Frontend.Models.dto;

namespace Savanna.Frontend.Controllers;

public class GameController : Controller
{
    private readonly BoardManager _boardManager;
    private readonly UIManager _uiManager;

    public GameController(BoardManager boardManager, UIManager uiManager)
    {
        _boardManager = boardManager;
        _uiManager = uiManager;
    }
    public IActionResult Index()
    {
        ViewData["UIManager"] = _uiManager;
        return View();
    }

    //invoked from view, adds animal to board and redirects to view
    [HttpPost]
    public IActionResult HandleInput([FromBody] RequestModel request)
    {
        var c = request.animalSymbol[0];
        if (AnimalFactory.AnimalTypes.ContainsKey(c))
        {
            Type animalType = AnimalFactory.AnimalTypes[c];
            _boardManager.AddAnimal(animalType);
        }
        else
        {
            Console.WriteLine("User provided symbol not found");
        }
        return Ok();   
    }

    [HttpGet]
    public IActionResult GetGameBoard()
    {
        _boardManager.MoveAnimals();
        var board = _uiManager.GetGameBoard();
        return Json(board);
    }
}
