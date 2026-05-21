using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public static class DbSeeder
{
    public static void Seed(MovieManagerDbContext context)
    {
        if (context.Categories.Any() || context.Actors.Any() || context.Movies.Any())
        {
            return;
        }

        Category action = new Category { Name = "Action" };
        Category drama = new Category { Name = "Drama" };
        Category sciFi = new Category { Name = "Sci-Fi" };

        Actor actorOne = new Actor
        {
            Name = "Ana Torres",
            Bio = "Award-winning actor known for action roles.",
            BirthDate = new DateTime(1987, 4, 12)
        };

        Actor actorTwo = new Actor
        {
            Name = "Luis Pereira",
            Bio = "Director and actor in independent films.",
            BirthDate = new DateTime(1979, 11, 2)
        };

        Actor actorThree = new Actor
        {
            Name = "Marta Silva",
            Bio = "Sci-fi specialist and theater performer.",
            BirthDate = new DateTime(1991, 8, 23)
        };

        Movie movieOne = new Movie
        {
            Title = "Edge of Tomorrowland",
            Director = "Sofia Mendes",
            ReleaseYear = 2022,
            Budget = 1500000,
            Category = sciFi,
            Actors = new List<Actor> { actorThree, actorOne }
        };

        Movie movieTwo = new Movie
        {
            Title = "City Shadows",
            Director = "Rafael Costa",
            ReleaseYear = 2020,
            Budget = 980000,
            Category = drama,
            Actors = new List<Actor> { actorTwo }
        };

        Movie movieThree = new Movie
        {
            Title = "Pulse Strike",
            Director = "Diego Alvarez",
            ReleaseYear = 2023,
            Budget = 2200000,
            Category = action,
            Actors = new List<Actor> { actorOne, actorTwo }
        };

        context.Categories.AddRange(action, drama, sciFi);
        context.Actors.AddRange(actorOne, actorTwo, actorThree);
        context.Movies.AddRange(movieOne, movieTwo, movieThree);
        context.SaveChanges();
    }
}
