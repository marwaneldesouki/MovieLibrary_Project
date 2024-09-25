using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLibrary_Project.Models;

namespace MovieLibrary_Project.Controllers
{
	public class MediaController : Controller
	{
		private readonly MovieLibraryContext _context;

		public MediaController(MovieLibraryContext context)
		{
			_context = context;
		}

		// Action to get Movie details
		public IActionResult MovieDetails(int id)
		{
			var movie = _context.Movies
				.Include(m => m.MediaGenres)
					.ThenInclude(mg => mg.Genre)
				.FirstOrDefault(m => m.Id == id);

			if (movie == null)
			{
				return NotFound();
			}

			return View(movie);
		}

		// Action to get Series details
		public IActionResult SeriesDetails(int id)
		{
			var series = _context.Series
				.Include(s => s.Seasons)
				.Include(s => s.MediaGenres)
					.ThenInclude(mg => mg.Genre)
				.FirstOrDefault(s => s.Id == id);

			if (series == null)
			{
				return NotFound();
			}

			return View(series);
		}	}

}
