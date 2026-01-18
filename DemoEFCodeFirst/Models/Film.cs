namespace DemoEFCodeFirst.Models;

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int ReleasedYear { get; set; }
    public int CreatorId { get; set; }
    public int Duration { get; set; }

    public Creator Creator { get; set; } = null!;
    public ICollection<Actor> Actors { get; set; } = [];
    
}
