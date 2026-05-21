using BusinessLogic.Dtos;
using BusinessLogic.Exceptions;
using Domain;
using Repository;

namespace BusinessLogic;

public class MovieService
{
    private readonly IMovieRepository _moviesRepository;

    public MovieService(IMovieRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }
    
    public MovieDTO AddMovie(MovieDTO movie)
    {
        ValidateMovieName(movie);
        Movie newMovie = _moviesRepository.Add(movie.toEntity());
        return MovieDTO.fromEntity(newMovie);
    }

    public IList<MovieDTO> GetAll()
    {
        return _moviesRepository.FindAll().Select(x => MovieDTO.fromEntity(x)).ToList();
    }

    private void ValidateMovieName(MovieDTO movie)
    {
        if (_moviesRepository.Find(x => x.Title == movie.Title) != null)
        {
            throw new LogicException("Can't add movie with same Title");
        }
    }
    
    public MovieDTO FindById(int id)
    {
        return MovieDTO.fromEntity(_moviesRepository.Find(x => x.Id == id));
    }

    public MovieDTO Update(MovieDTO updatedMovie)
    {
        return MovieDTO.fromEntity(_moviesRepository.Update(updatedMovie.toEntity()));
    }

    public void Delete(int id)
    {
        _moviesRepository.Delete(id);
    }
}
