using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savanna.Frontend.Models;

public class Game
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? AnimalsJson { get; set; }
    public int IterationCount { get; set; }
}
