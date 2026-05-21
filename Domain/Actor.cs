namespace Domain;

public class Actor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Bio { get; set; }

    public DateTime? BirthDate { get; set; }

    public List<Movie> Movies { get; set; } = new List<Movie>();
    
    public void Update(Actor updatedActor)
    {
        Id = updatedActor.Id;
        Name = updatedActor.Name;
        Bio = updatedActor.Bio;
        BirthDate = updatedActor.BirthDate;
    }
}
