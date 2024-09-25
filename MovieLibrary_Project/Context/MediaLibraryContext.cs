using Microsoft.EntityFrameworkCore;
using MovieLibrary_Project.Models;
using System.Reflection.Emit;

public class MediaLibraryContext : DbContext
{
	public MediaLibraryContext(DbContextOptions<MediaLibraryContext> options) : base(options)
	{
	}

  public DbSet<Media> Media { get; set; }
    public DbSet<Cast> Casts { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Episode> Episodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>()
            .HasMany(m => m.Casts)
            .WithOne(c => c.Media)
            .HasForeignKey(c => c.MediaId);

        modelBuilder.Entity<Media>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Media);

        modelBuilder.Entity<Season>()
            .HasMany(s => s.Episodes)
            .WithOne(e => e.Season)
            .HasForeignKey(e => e.SeasonId);
	// Seeding data
	modelBuilder.Entity<Genre>().HasData(

		new Genre { Id = 1, Name = "Action" },
        new Genre { Id = 2, Name = "Drama" },
        new Genre { Id = 3, Name = "Comedy" }
    );
		// Seed Media Data First
		modelBuilder.Entity<Media>().HasData(
			new Media
			{
				Id = 1,
				Title = "Inception",
				Description = "A thief who steals corporate secrets through the use of dream-sharing technology.",
				Rating = 8.8,
				Director = "Christopher Nolan",
				Writer = "Christopher Nolan",
				ReleaseDate = new DateTime(2010, 7, 16),
				PosterPath = "path_to_inception_poster.jpg",
				IsMovie = true
			},
			new Media
			{
				Id = 2,
				Title = "The Matrix",
				Description = "A computer hacker learns about the true nature of his reality.",
				Rating = 8.7,
				Director = "Lana Wachowski, Lilly Wachowski",
				Writer = "Lana Wachowski, Lilly Wachowski",
				ReleaseDate = new DateTime(1999, 3, 31),
				PosterPath = "path_to_matrix_poster.jpg",
				IsMovie = true
			},
			new Media
			{
				Id = 3,
				Title = "Breaking Bad",
				Description = "A high school chemistry teacher turned methamphetamine manufacturer.",
				Rating = 9.5,
				Director = "Vince Gilligan",
				Writer = "Vince Gilligan",
				ReleaseDate = new DateTime(2008, 1, 20),
				PosterPath = "path_to_breaking_bad_poster.jpg",
				IsMovie = false
			}
		);

		// Seed Cast Data Next
		modelBuilder.Entity<Cast>().HasData(
			new Cast { Id = 1, MediaId = 3, NameInMedia = "Walter White", RealName = "Bryan Cranston" ,PersonImage	=""},
			new Cast { Id = 2, MediaId = 3, NameInMedia = "Jesse Pinkman", RealName = "Aaron Paul", PersonImage = "" },
			new Cast { Id = 3, MediaId = 1, NameInMedia = "Dom Cobb", RealName = "Leonardo DiCaprio", PersonImage = "" },
			new Cast { Id = 4, MediaId = 1, NameInMedia = "Robert Fischer", RealName = "Cillian Murphy", PersonImage = "" },
			new Cast { Id = 5, MediaId = 2, NameInMedia = "Neo", RealName = "Keanu Reeves", PersonImage = "" },
			new Cast { Id = 6, MediaId = 2, NameInMedia = "Morpheus", RealName = "Laurence Fishburne", PersonImage = "" }
		);

		// Seed Season Data
		modelBuilder.Entity<Season>().HasData(
			new Season { Id = 1, SeasonNumber = 1, MediaId = 3 },
			new Season { Id = 2, SeasonNumber = 2, MediaId = 3 }
		);

		// Seed Episode Data
		modelBuilder.Entity<Episode>().HasData(
			new Episode { Id = 1, EpisodeNumber = 1, Title = "Pilot", SeasonId = 1 },
			new Episode { Id = 2, EpisodeNumber = 2, Title = "Cat's in the Bag...", SeasonId = 1 },
			new Episode { Id = 3, EpisodeNumber = 3, Title = "...And the Bag's in the River", SeasonId = 1 },
			new Episode { Id = 4, EpisodeNumber = 4, Title = "Cancer Man", SeasonId = 1 },
			new Episode { Id = 5, EpisodeNumber = 5, Title = "Gray Matter", SeasonId = 1 },
			new Episode { Id = 6, EpisodeNumber = 6, Title = "Crazy Handful of Nothin'", SeasonId = 1 },
			new Episode { Id = 7, EpisodeNumber = 7, Title = "A No-Rough-Stuff-Type Deal", SeasonId = 1 },
			new Episode { Id = 8, EpisodeNumber = 1, Title = "Seven Thirty-Seven", SeasonId = 2 },
			new Episode { Id = 9, EpisodeNumber = 2, Title = "Grilled", SeasonId = 2 },
			new Episode { Id = 10, EpisodeNumber = 3, Title = "Bit by a Dead Bee", SeasonId = 2 },
			new Episode { Id = 11, EpisodeNumber = 4, Title = "Down", SeasonId = 2 },
			new Episode { Id = 12, EpisodeNumber = 5, Title = "Breakage", SeasonId = 2 },
			new Episode { Id = 13, EpisodeNumber = 6, Title = "Peekaboo", SeasonId = 2 },
			new Episode { Id = 14, EpisodeNumber = 7, Title = "ABQ", SeasonId = 2 }
		);
	}
}
