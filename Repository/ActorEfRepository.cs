using System.Linq.Expressions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ActorEfRepository : IActorRepository
{
    private readonly MovieManagerDbContext _context;

    public ActorEfRepository(MovieManagerDbContext context)
    {
        _context = context;
    }

    public Actor Add(Actor actor)
    {
        _context.Actors.Add(actor);
        _context.SaveChanges();
        return actor;
    }

    public Actor? Find(Expression<Func<Actor, bool>> filter)
    {
        return _context.Actors.FirstOrDefault(filter);
    }

    public IList<Actor> FindAll()
    {
        return _context.Actors.ToList();
    }

    public Actor? Update(Actor updatedEntity)
    {
        Actor? existingActor = _context.Actors.FirstOrDefault(actor => actor.Id == updatedEntity.Id);
        if (existingActor is null)
        {
            return null;
        }

        existingActor.Update(updatedEntity);
        _context.SaveChanges();
        return existingActor;
    }

    public void Delete(int id)
    {
        Actor? existingActor = _context.Actors.FirstOrDefault(actor => actor.Id == id);
        if (existingActor is null)
        {
            return;
        }

        _context.Actors.Remove(existingActor);
        _context.SaveChanges();
    }
}
