namespace DemoEFCodeFirst.Models;

public class Actor
{
    public int Id { get; set; }
    public string Lastname { get; set; } = null!;
    public string Firstname { get; set; } = null!;

    public ICollection<Film> Films { get; set; } = [];
}