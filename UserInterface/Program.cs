using BusinessLogic;
using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<MovieManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieManager")));
builder.Services.AddScoped<IActorRepository, ActorEfRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryEfRepository>();
builder.Services.AddScoped<IMovieRepository, MovieEfRepository>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<ActorService>();
builder.Services.AddScoped<CategoryService>();


var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    MovieManagerDbContext dbContext = scope.ServiceProvider.GetRequiredService<MovieManagerDbContext>();
    dbContext.Database.Migrate();
    DbSeeder.Seed(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
