namespace osrs_farming_herblore_calc.Models;

public class ProfitCalculation : IComparable<ProfitCalculation>
{
    public required string Name {get; set;}
    public required decimal BreakEvenPoint {get; set;}
    public required decimal ProfitPer8Herbs {get; set;}

    public int CompareTo(ProfitCalculation? other)
    {
        if(this.ProfitPer8Herbs.CompareTo(other?.ProfitPer8Herbs) != 0)
        {
            return this.ProfitPer8Herbs.CompareTo(other?.ProfitPer8Herbs);
        }

        return 0;
    }
}