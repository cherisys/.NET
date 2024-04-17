using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameDbContext>(options => 
                                             options.UseSqlite(builder.Configuration.GetConnectionString("GameStoreConn")));

builder.Services.AddCors(options => 
    options.AddPolicy("GameWebPolicy", option =>
    {
        option.WithOrigins("https://localhost:7233").AllowAnyHeader().AllowAnyMethod();
    })
);

var app = builder.Build();
app.UseCors("GameWebPolicy");
app.MapGameEndpoints();
app.MapGenreEndpoints();
await app.MigrateDbAsync();
app.Run();
