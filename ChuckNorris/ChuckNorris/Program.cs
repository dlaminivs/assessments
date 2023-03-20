using ChuckNorris.Areas.Identity.Data;
using ChuckNorris.Data;
using ChuckNorris.Data.DataManager;
using ChuckNorris.Models;
using ChuckNorris.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
builder.Services.AddDbContext<JokeContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddTransient<IRepository<Joke>, Repository<Joke, JokeContext>>();
builder.Services.AddTransient<IRepository<UserFavouriteJoke>, Repository<UserFavouriteJoke, JokeContext>>();
builder.Services.AddHttpClient<IJokeService, JokeService>();
builder.Services.AddScoped<IFavouriteJokeService, FavouriteJokeService>();
builder.Services.AddDefaultIdentity<ChuckNorrisUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<JokeContext>();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
