namespace DemoEFCodeFirst.Models;

public class FilmActor
{
    public int FilmId { get; set; }
    public int ActorId { get; set; }

    public string? CharacterLastname { get; set; }
    public string CharacterFirstname { get; set; } = null!;

    public Film Film { get; set; } = null!;
    public Actor Actor { get; set; } = null!;

}
