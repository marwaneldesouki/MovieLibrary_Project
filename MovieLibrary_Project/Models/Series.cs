namespace MovieLibrary_Project.Models
{
	public class Series : Media
	{
		public ICollection<Season> Seasons { get; set; }
	}


}
