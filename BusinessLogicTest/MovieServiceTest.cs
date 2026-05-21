using BusinessLogic;
using BusinessLogic.Dtos;
using BusinessLogic.Exceptions;
using Domain;
using Domain.Exceptions;
using Repository;

namespace BusinessLogicTest;

[TestClass]
public class MovieServiceTest
{

    private MovieService _movieService;
    private IMovieRepository _movieRepository;
    
    [TestInitialize]
    public void SetUp()
    {
        _movieRepository = new MovieMemoryRepository();
        _movieService = new MovieService(_movieRepository);
    }
    
    [TestMethod]
    public void AddOneMovieOkTest()
    {
        MovieDTO movie = new MovieDTO()
        {
            Title = "Avatar",
            Director = "One Director",
            ReleaseYear = 2020
        };
        
        MovieDTO returnMovie = _movieService.AddMovie(movie);
        
        Assert.AreEqual(1, returnMovie.Id);
        Assert.AreEqual(movie.Title, returnMovie.Title);
        Assert.AreEqual(movie.Director, returnMovie.Director);
        Assert.AreEqual(movie.ReleaseYear, returnMovie.ReleaseYear);
    }
    
    [TestMethod]
    public void AddTwoMovieOkTest()
    {
        MovieDTO movieOne = new MovieDTO()
        {
            Title = "Avatar",
            Director = "One Director",
            ReleaseYear = 2020
        };
        MovieDTO movieTwo = new MovieDTO()
        {
            Title = "Avatar 2",
            Director = "One Director",
            ReleaseYear = 2020
        };

        MovieDTO returnMovieOne = _movieService.AddMovie(movieOne);
        MovieDTO returnMovieTwo = _movieService.AddMovie(movieTwo);

        Assert.AreEqual(1, returnMovieOne.Id);        
        Assert.AreEqual(2, returnMovieTwo.Id);
    }
    
    [TestMethod]
    public void ListAllMovieOkTest()
    {
        MovieDTO movieOne = new MovieDTO()
        {
            Title = "Avatar",
            Director = "One Director",
            ReleaseYear = 2020
        };
        MovieDTO movieTwo = new MovieDTO()
        {
            Title = "Avatar 2",
            Director = "One Director",
            ReleaseYear = 2020
        };

        MovieDTO returnMovieOne = _movieService.AddMovie(movieOne);
        MovieDTO returnMovieTwo = _movieService.AddMovie(movieTwo);

        IList<MovieDTO> resultMovies = _movieService.GetAll();

        Assert.AreEqual("Avatar", resultMovies.FirstOrDefault(x => x.Id == 1).Title);        
        Assert.AreEqual("Avatar 2", resultMovies.FirstOrDefault(x => x.Id == 2).Title);        

    }
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddOneMovieTitleEmptyTest()
    {
        Movie movie = new Movie()
        {
            Title = "",
            Director = "One Director",
            ReleaseYear = 2020,
            Budget = 25000
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddOneMovieTitleNullTest()
    {
        Movie movie = new Movie()
        {
            Title = null,
            Director = "One Director",
            ReleaseYear = 2020,
            Budget = 300000
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddOneMovieBudgetNullTest()
    {
        Movie movie = new Movie()
        {
            Title = "Avatar",
            Director = "One Director",
            ReleaseYear = 2020,
            Budget = -500000
        };
    }

    [TestMethod]
    public void AddMovieSameNameOkTest()
    {
        MovieDTO movieOne = new MovieDTO()
        {
            Title = "Avatar",
            Director = "One Director",
            ReleaseYear = 2020
        };
        MovieDTO movieTwo = new MovieDTO()
        {
            Title = "Avatar",
            Director = "One Director",
            ReleaseYear = 2020
        };

        _movieService.AddMovie(movieOne);

        var exception = Assert.ThrowsException<LogicException>(() => _movieService.AddMovie(movieTwo));
        Assert.AreEqual("Can't add movie with same Title", exception.Message);
    }
}
