using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieLibrary_Project.Models
{
    public class MediaCreateViewModel
    {
            public Media Media { get; set; } // For binding Media details
            public List<SelectListItem> AvailableCasts { get; set; } // For the cast dropdown
            public List<SelectListItem> AvailableGenres { get; set; } // For the genre dropdown
            public List<string> SelectedCasts { get; set; } = new List<string>();
            public List<string> SelectedGenres { get; set; } = new List<string>();
            public bool IsMovie { get; set; } // To determine if the media is a movie
            public int SeasonNumber { get; set; } // For adding seasons
            public int NumberOfEpisodes { get; set; } // For adding episodes
       


    }
}
