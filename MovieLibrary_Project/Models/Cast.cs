namespace MovieLibrary_Project.Models
{
	public class Cast
	{
		public int Id { get; set; }
		public string NameInMedia { get; set; }
		public string RealName { get; set; }
		public string PersonImage {  get; set; }

		// Foreign Key
		public int MediaId { get; set; }
		public Media Media { get; set; }
	}
}
