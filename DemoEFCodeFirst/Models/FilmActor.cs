namespace DemoEFCodeFirst.Models;

public class FilmActor
{
    public int FilmId { get; set; }
    public int ActorId { get; set; }

    public Film Film { get; set; } = null!;
    public Actor Actor { get; set; } = null!;

}
