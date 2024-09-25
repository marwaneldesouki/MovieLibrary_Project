namespace MovieLibrary_Project.Models
{
	public class Episode
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int EpisodeNumber { get; set; }

		// Foreign Key
		public int SeasonId { get; set; }
		public Season Season { get; set; }
	}
}
