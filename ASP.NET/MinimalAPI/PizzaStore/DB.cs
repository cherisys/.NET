namespace PizzaStore.DB
{
    public record Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }    
    }

    public class PizzaDB
    {
        private static List<Pizza> pizzas = new List<Pizza>()
        {
             new Pizza{ Id=1, Name="Montemagno, Pizza shaped like a great mountain" },
             new Pizza{ Id=2, Name="The Galloway, Pizza shaped like a submarine, silent but deadly"},
             new Pizza{ Id=3, Name="The Noring, Pizza shaped like a Viking helmet, where's the mead"}
        };


        public static List<Pizza> GetPizzas()
        {
            return pizzas;
        }

        public static Pizza? GetPizza(int id)
        {
            return pizzas.SingleOrDefault(p => p.Id == id);
        }

        public static Pizza CreatePizza(Pizza pizza)
        {
            pizzas.Add(pizza);
            return pizza;
        }

        public static Pizza UpdatePizza(Pizza updatedPizza)
        {
            //var pizza = pizzas.SingleOrDefault(p => p.Id == updatedPizza.Id);
            //if (pizza == null)
            //{
            //    pizza.Name = updatedPizza.Name;
            //}

            pizzas = pizzas.Select(pizza => {
                if (pizza.Id == updatedPizza.Id)
                {
                    pizza.Name = updatedPizza.Name;
                }
                return pizza;
            }).ToList();
            return updatedPizza;

        }

        public static void DeletePizza(int id) 
        {
            var pizza = GetPizza(id);
            pizzas.Remove(pizza);
        }
    }
}
