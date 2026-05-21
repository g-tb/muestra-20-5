using BusinessLogic.Dtos;
using Domain;
using Repository;

namespace BusinessLogic;

public class ActorService
{
    private readonly IActorRepository _repository;

    public ActorService(IActorRepository actorRepository)
    {
        _repository = actorRepository;
    }
    public ActorDTO AddMovie(Actor movie)
    {
        return ActorDTO.fromEntity(_repository.Add(movie));
    }

    public IList<ActorDTO> GetAll()
    {
        return _repository.FindAll().Select(x => ActorDTO.fromEntity(x)).ToList();
    }
    
    public ActorDTO FindById(int id)
    {
        return ActorDTO.fromEntity(_repository.Find(x => x.Id == id));
    }

    public ActorDTO Update(ActorDTO updatedActor)
    {
        return ActorDTO.fromEntity(_repository.Update(updatedActor.toEntity()));
    }

    public void Delete(int id)
    {
        _repository.Delete(id);
    }
}
