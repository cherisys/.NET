using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddDbContext<PizzaDB>(options => options.UseInMemoryDatabase("PizzaDB"));

var connectinString = builder.Configuration.GetConnectionString("PizzaDB") ?? "Data Source=PizzaDB.db";
builder.Services.AddSqlite<PizzaDB>(connectinString);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API v1", Description = "Making pizzas that you love.", Version = "v1"});
});


string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
      builder =>
      {
          builder.WithOrigins(
            "http://localhost.com:5173", "*");
      });
});

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
app.UseSwagger();
app.UseSwaggerUI(c=>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json","PizzaStore API v1");
});

app.MapGet("/", () => "Hello World");
app.MapGet("/pizzas", async (PizzaDB db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizzas/{id}", async (PizzaDB db, int id) => await db.Pizzas.FindAsync(id));
app.MapPost("/pizzas", async (PizzaDB db, Pizza pizza) => 
{ 
    await db.Pizzas.AddAsync(pizza); 
    await db.SaveChangesAsync();
    return Results.Created($"/pizzas/{pizza.Id}", pizza);
});
app.MapPut("/pizzas/{id}", async (PizzaDB db, Pizza updatedPizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza == null) return Results.NotFound();
    pizza.Name = updatedPizza.Name;
    pizza.Description = updatedPizza.Description;
    db.Pizzas.Update(pizza);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/pizzas/{id}", async (PizzaDB db, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();
    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.Run();
