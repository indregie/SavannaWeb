using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Savanna.Backend;
using Savanna.Backend.Interfaces;
using Savanna.Frontend;
using Savanna.Frontend.Data;
using Savanna.Frontend.Interfaces;
using Savanna.Frontend.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString)
    );

builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("IdentityOptions"));
//identity to be able to use EF
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IBoardManager, BoardManager>();
builder.Services.AddSingleton<IUIManager, UIManager>();
builder.Services.AddScoped<DataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Game}/{action=Index}/{id?}");

app.Run();
