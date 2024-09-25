using Microsoft.AspNetCore.Mvc.Rendering;
using MovieLibrary_Project.Models;
namespace MovieLibrary_Project.Models;

public class MediaEditViewModel
{
    public Media Media { get; set; }
    public List<SelectListItem> AvailableCasts { get; set; }
    public List<string> SelectedCasts { get; set; } = new List<string>();
    public List<SelectListItem> AvailableGenres { get; set; }
    public List<string> SelectedGenres { get; set; } = new List<string>();
}
