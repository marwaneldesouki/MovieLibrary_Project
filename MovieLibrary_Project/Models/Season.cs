namespace MovieLibrary_Project.Models
{
	public class Season
	{
		public int Id { get; set; }
		public int SeasonNumber { get; set; }

		// Foreign Key
		public int MediaId { get; set; }
		public Media Media { get; set; }

		// Navigation Property
		public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
	}
}
