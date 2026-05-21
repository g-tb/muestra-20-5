using Domain;
using System.Linq.Expressions;

namespace Repository;

public class ActorMemoryRepository : IActorRepository
{

    private List<Actor> _actors = new List<Actor>();
    public Actor Add(Actor oneElement)
    {
        oneElement.Id = _actors.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _actors.Add(oneElement);
        return oneElement;
    }

    public Actor? Find(Expression<Func<Actor, bool>> filter)
    {
        return _actors.AsQueryable().Where(filter).FirstOrDefault();
    }

    public IList<Actor> FindAll()
    {
        return _actors;
    }

    public Actor? Update(Actor updatedEntity)
    {
        Actor findActor = Find(x => x.Id == updatedEntity.Id);
        findActor = updatedEntity;
        return findActor;
    }

    public void Delete(int id)
    {
        _actors.RemoveAll(x => x.Id == id);
    }
}
