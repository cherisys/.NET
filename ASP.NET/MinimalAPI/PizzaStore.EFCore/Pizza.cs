using Microsoft.EntityFrameworkCore;

namespace PizzaStore.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class PizzaDB:DbContext
    {
        public PizzaDB(DbContextOptions<PizzaDB> options) : base(options) { }
        public DbSet<Pizza> Pizzas { get; set; }
    }
}
