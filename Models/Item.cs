namespace osrs_farming_herblore_calc.Models;

public class Item
{
    public decimal Price {get; set;}

    public string Name {get; set;}

    public int Id {get; set;}

    public Item(string name, int id)
    {
        Name = name;
        Id = id;
        Price = 0m;
    }
}