namespace MovieLibrary_Project.Models
{
	public class Genre
	{
		public int Id { get; set; }
		public string Name { get; set; }

		// Navigation Property
		public ICollection<Media> Media { get; set; } = new List<Media>();
	}

}
