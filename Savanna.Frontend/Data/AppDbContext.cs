using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Savanna.Frontend.Models;

namespace Savanna.Frontend.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Game> Games { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
