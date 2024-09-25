namespace MovieLibrary_Project.Models
{
	public class Media
	{
       
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public double Rating { get; set; }
            public string Director { get; set; }
            public string Writer { get; set; }
            public DateTime ReleaseDate { get; set; }
            public string PosterPath { get; set; }

            // Navigation Properties
            public ICollection<Cast> Casts { get; set; } = new List<Cast>();
            public ICollection<Genre> Genres { get; set; } = new List<Genre>();
            public ICollection<Season> Seasons { get; set; } = new List<Season>(); // Initialize here
            public bool IsMovie { get; set; }

    }

}
