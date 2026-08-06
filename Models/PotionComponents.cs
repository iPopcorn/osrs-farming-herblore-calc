namespace osrs_farming_herblore_calc.Models;

public class PotionComponents
{
    public string Name {get; set;}
    public Item Potion {get; set;}
    public Item Seed {get; set;}
    // TODO: Handle secondaries
    public Item? Secondary {get; set;}

    public PotionComponents(string name, Item potion, Item seed)
    {
        Name = name;
        Potion = potion;
        Seed = seed;
    }
}