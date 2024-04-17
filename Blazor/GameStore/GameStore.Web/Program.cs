using GameStore.Web;
using GameStore.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7141") });
builder.Services.AddScoped<IGameService,GameService>();
builder.Services.AddScoped<IGenreService, GenreService>();
await builder.Build().RunAsync();
