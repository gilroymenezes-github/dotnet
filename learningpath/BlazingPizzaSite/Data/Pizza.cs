namespace BlazingPizzaSite;

public class Pizza
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price {get;set;} = 0.00M;
    public bool IsVegetarian {get;set;} = default(bool);
}