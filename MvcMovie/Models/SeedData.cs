
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Linq;

namespace MvcMovie.Models;

public static class SeedData
{
	public static void Initialize(IServiceProvider serviceProvider)
	{
		using (var context = new MvcMovieContext(
			serviceProvider.GetRequiredService<
				DbContextOptions<MvcMovieContext>>()))
		{
			// Look for any movies.
			if (context.Movie.Any())
			{
				return;   // DB has been seeded
			}
			context.Movie.AddRange(
				new Movie
				{
					Title = "When Harry Met Sally",
					Poster = "https://m.media-amazon.com/images/M/MV5BMjE0ODEwNjM2NF5BMl5BanBnXkFtZTcwMjU2Mzg3NA@@._V1_.jpg",
					ReleaseDate = DateTime.Parse("1989-2-12"),
					Genre = "Romantic Comedy",
					Price = 7.99M,
					Rating = "R"
				},
				new Movie
				{
					Title = "Ghostbusters ",
					Poster = "https://m.media-amazon.com/images/M/MV5BMjY4MDg0OGItOTBlYi00Y2M1LTljNzMtY2I3M2QzOGQzNWJhXkEyXkFqcGdeQXVyNDkzNTM2ODg@._V1_.jpg",
					ReleaseDate = DateTime.Parse("1984-3-13"),
					Genre = "Comedy",
					Price = 8.99M,
					Rating = "PG"
				},
				new Movie
				{
					Title = "Ace Ventura: Pet Detective",
					Poster = "https://m.media-amazon.com/images/M/MV5BYmVhNmFmOGYtZjgwNi00ZGQ0LThiMmQtOGZjMDUzNzJhMGIzXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_FMjpg_UY1600_.jpg",
					ReleaseDate = DateTime.Parse("1994-2-23"),
					Genre = "Comedy",
					Price = 9.99M,
					Rating = "PG-13"
				},
				new Movie
				{
					Title = "Rio Bravo",
					Poster = "https://m.media-amazon.com/images/M/MV5BMjgzZTFlY2ItNmUyMS00YzUyLTgzM2QtMDhhYTgyODEzMWFmXkEyXkFqcGdeQXVyMDUyOTUyNQ@@._V1_.jpg",
					ReleaseDate = DateTime.Parse("1959-4-15"),
					Genre = "Western",
					Price = 3.99M,
					Rating = "PG"
				},
				new Movie
				{
					Title = "Se7en",
					Poster = "https://m.media-amazon.com/images/M/MV5BOTUwODM5MTctZjczMi00OTk4LTg3NWUtNmVhMTAzNTNjYjcyXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg",
					ReleaseDate = DateTime.Parse("1995-4-15"),
					Genre = "Mystery Thriller",
					Price = 3.99M,
					Rating = "R"
				},
				new Movie
				{
					Title = "The Silence Of The Lambs",
					Poster = "https://m.media-amazon.com/images/M/MV5BNjNhZTk0ZmEtNjJhMi00YzFlLWE1MmEtYzM1M2ZmMGMwMTU4XkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg",
					ReleaseDate = DateTime.Parse("1991-4-15"),
					Genre = "Crime Thriller",
					Price = 3.99M,
					Rating = "R"
				}
			);
			context.SaveChanges();
		}
	}
}
