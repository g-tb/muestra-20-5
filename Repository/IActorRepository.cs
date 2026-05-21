using System.Linq.Expressions;
using Domain;

namespace Repository;

public interface IActorRepository
{
    Actor Add(Actor actor);
    Actor? Find(Expression<Func<Actor, bool>> filter);
    IList<Actor> FindAll();
    Actor? Update(Actor updatedEntity);
    void Delete(int id);
}
