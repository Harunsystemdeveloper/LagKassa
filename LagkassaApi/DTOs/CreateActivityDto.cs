namespace LagkassaApi.DTOs;

public class CreateActivityDto
{
    public string Name { get; set; } = "";
    public DateTime Date { get; set; }
    public decimal TotalCost { get; set; }
    public List<int> CalledPlayerIds { get; set; } = new();
}
