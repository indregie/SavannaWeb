using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Savanna.Backend;
using Savanna.Backend.Actors;
using Savanna.Backend.Interfaces;
using Savanna.Frontend.Interfaces;
using Savanna.Frontend.Models;
using Savanna.Frontend.Models.dto;

namespace Savanna.Frontend.Controllers;

//[Authorize]
public class GameController : Controller
{
    private readonly IBoardManager _boardManager;
    private readonly IDrawingService _drawingService;
    private readonly IDataService _dataService;
    private readonly UserManager<AppUser> _userManager;

    public GameController(IBoardManager boardManager, IDrawingService drawingService, IDataService dataService, UserManager<AppUser> userManager)
    {
        _boardManager = boardManager;
        _drawingService = drawingService;
        _dataService = dataService;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        ViewData["UIManager"] = _drawingService;
        return View();
    }

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
        var board = _drawingService.GetGameBoard();
        return Json(new
        {
            iterationCount = _boardManager.IterationCount,
            animals = _boardManager.Animals,
            board = board
        });
    }

    [HttpPost]
    public async Task<IActionResult> SaveGame()
    {
        try
        {
            var animals = _boardManager.GetBoardAnimals();
            var animalsJson = JsonConvert.SerializeObject(animals);
            var iterationCount = _boardManager.IterationCount;
            var game = new Game();
            game.AnimalsJson = animalsJson;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return StatusCode(500, "Unexpected error occured.");
            }
            await _dataService.SaveGame(userId, animalsJson, iterationCount);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save game {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> LoadGame()
    {
        try
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return StatusCode(500, "Unexpected error occured.");
            }
            var game = await _dataService.LoadGame(userId);

            if (game == null)
            {
                return NotFound("No game found for the user.");
            }

            _boardManager.ClearAnimals();

            if (game.AnimalsJson == null)
            {
                return StatusCode(500, "Game data is null.");
            }

            List<JObject>? jsonAnimals = JsonConvert.DeserializeObject<List<JObject>>(game.AnimalsJson);

            if (jsonAnimals == null)
            {
                return StatusCode(500, "Failed to deserialize game data");
            }

            foreach (var jsonAnimal in jsonAnimals)
            {
                char animalSymbol = jsonAnimal["Symbol"]?.ToString()[0] ??
                    throw new InvalidOperationException("Animal symbol is null or empty.");
                Type? animalType = AnimalFactory.AnimalTypes[animalSymbol];
                if (animalType == null)
                {
                    await Console.Out.WriteLineAsync($"Animal type not found for symbol {animalSymbol}");
                    continue;
                }
                Animal? animal = (Animal)jsonAnimal.ToObject(animalType)!;
                if (animal != null)
                {
                    _boardManager.Animals.Add(animal);
                }
            }
            _boardManager.IterationCount = game.IterationCount;
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

    [HttpGet("/animalIcon/{animalSymbol}")]
    public IActionResult AnimalIcon([FromRoute] string animalSymbol)
    {       
        var animalSymbolCh = animalSymbol[0];
        var imgBytes = AnimalFactory.AnimalIcons[animalSymbolCh];

        return File(imgBytes, "image/png");
    }
}
