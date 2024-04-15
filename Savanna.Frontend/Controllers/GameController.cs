using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savanna.Backend;
using Savanna.Backend.Interfaces;
using Savanna.Frontend.Interfaces;
using Savanna.Frontend.Models;
using Savanna.Frontend.Models.dto;
using System.Text.Json;

namespace Savanna.Frontend.Controllers;

//[Authorize]
public class GameController : Controller
{
    private readonly IBoardManager _boardManager;
    private readonly IUIManager _uiManager;
    private readonly DataService _dataService;
    private readonly UserManager<AppUser> _userManager;

    public GameController(IBoardManager boardManager, IUIManager uiManager, DataService dataService, UserManager<AppUser> userManager)
    {
        _boardManager = boardManager;
        _uiManager = uiManager;
        _dataService = dataService;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        ViewData["UIManager"] = _uiManager;
        ViewData["by"] = 123;
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

    [HttpPost]
    public async Task<IActionResult> SaveGame()
    {
        try
        {
            var animals = _boardManager.GetBoardAnimals();
            var animalsJson = JsonSerializer.Serialize(animals);
            var game = new Game();
            game.AnimalsJson = animalsJson;
            var userId = _userManager.GetUserId(User);
            await _dataService.SaveGame(userId, animalsJson);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save game {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public IActionResult NewGame()
    {
        try
        {
            _boardManager.ClearAnimals();
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start new game {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}
