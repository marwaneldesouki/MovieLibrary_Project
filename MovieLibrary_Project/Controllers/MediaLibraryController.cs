using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLibrary_Project.Models;

public class MediaLibraryController : Controller
{
	private readonly MediaLibraryContext _context;

	public MediaLibraryController(MediaLibraryContext context)
	{
		_context = context;
	}

	// GET: Media
	public async Task<IActionResult> Index()
	{
		var mediaList = await _context.Media
			.Include(m => m.Casts)
			.Include(m => m.Genres)
			.ToListAsync();
		return View(mediaList);
	}

	// GET: Media/Movies
	public async Task<IActionResult> Movies()
	{
		var movies = await _context.Media
			.Where(m => m.IsMovie) // Assuming IsMovie is a boolean property in your Media model
			.Include(m => m.Casts)
			.Include(m => m.Genres)
			.ToListAsync();
		return View(movies);
	}

	// GET: Media/Series
	public async Task<IActionResult> Series()
	{
		var series = await _context.Media
			.Where(m => !m.IsMovie) // Assuming IsMovie is a boolean property in your Media model
			.Include(m => m.Casts)
			.Include(m => m.Genres)
			.ToListAsync();
		return View(series);
	}

	// GET: Media/Details/5
	// GET: Media/Details/5
	public async Task<IActionResult> Details(int id)
	{
		var media = await _context.Media
			.Include(m => m.Casts)
			.Include(m => m.Genres)
			.Include(m => m.Seasons)
			.ThenInclude(s => s.Episodes)
			.FirstOrDefaultAsync(m => m.Id == id);

		if (media == null)
		{
			return NotFound();
		}

		if (media.IsMovie)
		{
			return View("MovieDetails", media); // Render the MovieDetails view
		}
		else
		{
			return View("SeriesDetails", media); // Render the SeriesDetails view
		}
	}


	// GET: Media/Create
	public IActionResult Create()
	{
		return View();
	}

	// POST: Media/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Media media)
	{
		if (ModelState.IsValid)
		{
			_context.Add(media);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		return View(media);
	}

	// GET: Media/Edit/5
	public async Task<IActionResult> Edit(int id)
	{
		var media = await _context.Media.FindAsync(id);
		if (media == null)
		{
			return NotFound();
		}
		return View(media);
	}

	// POST: Media/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, Media media)
	{
		if (id != media.Id)
		{
			return NotFound();
		}

		if (ModelState.IsValid)
		{
			try
			{
				_context.Update(media);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MediaExists(media.Id))
				{
					return NotFound();
				}
				throw;
			}
			return RedirectToAction(nameof(Index));
		}
		return View(media);
	}

	// GET: Media/Delete/5
	public async Task<IActionResult> Delete(int id)
	{
		var media = await _context.Media
			.FirstOrDefaultAsync(m => m.Id == id);
		if (media == null)
		{
			return NotFound();
		}

		return View(media);
	}

	// POST: Media/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var media = await _context.Media.FindAsync(id);
		_context.Media.Remove(media);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	private bool MediaExists(int id)
	{
		return _context.Media.Any(e => e.Id == id);
	}
}
