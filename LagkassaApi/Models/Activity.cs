namespace LagkassaApi.Models;

public class Activity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime Date { get; set; }
    public decimal TotalCost { get; set; }
    public List<int> CalledPlayerIds { get; set; } = new();
    public decimal CostPerPlayer { get; set; }
}
