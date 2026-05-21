using System.Linq.Expressions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class MovieEfRepository : IMovieRepository
{
    private readonly MovieManagerDbContext _context;

    public MovieEfRepository(MovieManagerDbContext context)
    {
        _context = context;
    }

    public Movie Add(Movie movie)
    {
        AttachCategory(movie);
        AttachActors(movie);

        _context.Movies.Add(movie);
        _context.SaveChanges();
        return movie;
    }

    public Movie? Find(Expression<Func<Movie, bool>> filter)
    {
        return _context.Movies
            .Include(movie => movie.Category)
            .Include(movie => movie.Actors)
            .FirstOrDefault(filter);
    }

    public IList<Movie> FindAll()
    {
        return _context.Movies
            .Include(movie => movie.Category)
            .Include(movie => movie.Actors)
            .ToList();
    }

    public Movie? Update(Movie updatedEntity)
    {
        Movie? existingMovie = _context.Movies
            .Include(movie => movie.Category)
            .Include(movie => movie.Actors)
            .FirstOrDefault(movie => movie.Id == updatedEntity.Id);

        if (existingMovie is null)
        {
            return null;
        }

        existingMovie.Update(updatedEntity);
        AttachCategory(existingMovie);
        AttachActors(existingMovie);

        _context.SaveChanges();
        return existingMovie;
    }

    public void Delete(int id)
    {
        Movie? existingMovie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (existingMovie is null)
        {
            return;
        }

        _context.Movies.Remove(existingMovie);
        _context.SaveChanges();
    }

    private void AttachCategory(Movie movie)
    {
        if (movie.Category is null || movie.Category.Id <= 0)
        {
            return;
        }

        _context.Attach(movie.Category);
    }

    private void AttachActors(Movie movie)
    {
        if (movie.Actors.Count == 0)
        {
            return;
        }

        foreach (Actor actor in movie.Actors)
        {
            if (actor.Id <= 0)
            {
                continue;
            }

            _context.Attach(actor);
        }
    }
}
