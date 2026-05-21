using System.Linq.Expressions;
using Domain;

namespace Repository;

public interface IMovieRepository
{
    Movie Add(Movie movie);
    Movie? Find(Expression<Func<Movie, bool>> filter);
    IList<Movie> FindAll();
    Movie? Update(Movie updatedEntity);
    void Delete(int id);
}
