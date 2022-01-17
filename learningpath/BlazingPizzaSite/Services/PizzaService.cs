namespace BlazingPizzaSite;

public class PizzaService 
{
    public List<Pizza> GetPizzas()
    {
        return new List<Pizza>
        {
            new Pizza { Id = Guid.NewGuid().ToString(), Name = "The Godfather", Price = 11.99M, Description = "Just like the movies." },
            new Pizza { Id = Guid.NewGuid().ToString(), Name = "Hawaiian", Price = 12.99M, Description = "Mmmm pig'n'pineapple." },
            new Pizza { Id = Guid.NewGuid().ToString(), Name = "Margarita", Price = 8.99M, Description = "Tomato basil cheese.", IsVegetarian = true }
        };
    }
}