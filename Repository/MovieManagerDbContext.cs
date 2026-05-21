using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class MovieManagerDbContext : DbContext
{
    public MovieManagerDbContext(DbContextOptions<MovieManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(movie => movie.Id);
            entity.Property(movie => movie.Title).IsRequired();
            entity.Property(movie => movie.Budget).IsRequired();

            entity.HasOne(movie => movie.Category)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(movie => movie.Actors)
                .WithMany(actor => actor.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieActors",
                    join => join
                        .HasOne<Actor>()
                        .WithMany()
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join => join
                        .HasOne<Movie>()
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.HasKey("MovieId", "ActorId");
                        join.ToTable("MovieActors");
                    });
        });
    }
}
