using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieLibrary_Project.Models;

namespace MovieLibrary_Project.Controllers
{
    public class MediaAdminController : Controller
    {
        private readonly MediaLibraryContext _context;
        private readonly ILogger<MediaAdminController> _logger;
        public MediaAdminController(MediaLibraryContext context, ILogger<MediaAdminController> logger)
        {
            _context = context;
            _logger = logger;
        }
        private bool MediaExists(int id)
        {
            return _context.Media.Any(e => e.Id == id);
        }
        // Index: List all media (movies & series)
        public async Task<IActionResult> Index()
        {
            var mediaList = await _context.Media.ToListAsync();

            return View(mediaList);
        }

        // GET: Media/Create
        public IActionResult Create()
        {
            var viewModel = new MediaCreateViewModel
            {
                Media = new Media(), // Ensure Media is initialized
                AvailableCasts = _context.Casts
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.RealName
                    }).ToList(),
                AvailableGenres = _context.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }).ToList()
            };

            return View(viewModel);
        }


        // POST: Media/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MediaCreateViewModel viewModel)
        {
          
                var media = viewModel.Media;

                // Ensure media is properly initialized
                if (media == null)
                {
                    return BadRequest("Media is not initialized.");
                }

                // Adding selected casts
                if (viewModel.SelectedCasts != null)
                {
                    foreach (var castId in viewModel.SelectedCasts)
                    {
                        var cast = await _context.Casts.FindAsync(int.Parse(castId));
                        if (cast != null)
                        {
                            media.Casts.Add(cast);
                        }
                    }
                }

                // Adding selected genres
                if (viewModel.SelectedGenres != null)
                {
                    foreach (var genreId in viewModel.SelectedGenres)
                    {
                        var genre = await _context.Genres.FindAsync(int.Parse(genreId));
                        if (genre != null)
                        {
                            media.Genres.Add(genre);
                        }
                    }
                }

            // Check if the media is a movie or series
            if (!viewModel.IsMovie)
            {
                // Ensure the season name is not null or empty
                if (viewModel.SeasonNumber <= 0)
                {
                    ModelState.AddModelError(nameof(viewModel.SeasonNumber), "Season number is required.");
                    return View(viewModel);
                }

                // Handle series: add seasons and episodes
                var season = new Season
                {
                    SeasonNumber = viewModel.SeasonNumber,
                    Episodes = new List<Episode>()
                };

                // Ensure the number of episodes is valid
                if (viewModel.NumberOfEpisodes <= 0)
                {
                    ModelState.AddModelError(nameof(viewModel.NumberOfEpisodes), "Number of episodes must be greater than 0.");
                    return View(viewModel);
                }

                for (int i = 0; i < viewModel.NumberOfEpisodes; i++)
                {
                    season.Episodes.Add(new Episode
                    {
                        Title = $"Episode {i + 1}" // Customize episode titles as needed
                    });
                }

                // Correctly add the season to the media
                media.Seasons.Add(season); // Use Add instead of Append // Add the season to the media
            }
            else
                media.IsMovie = true;

                // Save the media to the database
                _context.Media.Add(media);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Redirect to Index after creating

            // If we got this far, something failed, redisplay the form
            viewModel.AvailableCasts = _context.Casts
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.RealName
                }).ToList();

            viewModel.AvailableGenres = _context.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                }).ToList();

            return View(viewModel); // Return the view with the model to display validation errors
        }



        // Edit: GET
        public async Task<IActionResult> Edit(int id)
        {
            var media = await _context.Media
                .Include(m => m.Casts)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (media == null)
            {
                return NotFound();
            }

            var viewModel = new MediaEditViewModel
            {
                Media = media,
                AvailableCasts = _context.Casts.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.RealName} (as {c.NameInMedia})"
                }).ToList(),
                AvailableGenres = _context.Genres.Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                }).ToList(),
                SelectedCasts = media.Casts.Select(c => c.Id.ToString()).ToList(),
                SelectedGenres = media.Genres.Select(g => g.Id.ToString()).ToList()
            };

            return View(viewModel);
        }


        // Edit: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MediaEditViewModel viewModel)
        {
            if (viewModel.Media.Id != viewModel.Media.Id)
            {
                return NotFound();
            }


            try
            {
                // Fetch the existing media entry
                var mediaToUpdate = await _context.Media
                    .Include(m => m.Casts)
                    .Include(m => m.Genres)
                    .FirstOrDefaultAsync(m => m.Id == viewModel.Media.Id);

                // Update the media properties
                mediaToUpdate.Title = viewModel.Media.Title;
                mediaToUpdate.Description = viewModel.Media.Description;
                mediaToUpdate.Rating = viewModel.Media.Rating;
                mediaToUpdate.Director = viewModel.Media.Director;
                mediaToUpdate.Writer = viewModel.Media.Writer;
                mediaToUpdate.ReleaseDate = viewModel.Media.ReleaseDate;
                mediaToUpdate.PosterPath = viewModel.Media.PosterPath;
                

                // Update Casts
                foreach (var castId in viewModel.SelectedCasts)
                {
                    var cast = await _context.Casts.FindAsync(int.Parse(castId));
                    if (cast != null && !mediaToUpdate.Casts.Any(c => c.Id == cast.Id))
                    {
                        
                        mediaToUpdate.Casts.Add(cast); // Add only if not already present
                    }
                }

                // Update Genres
                foreach (var genreId in viewModel.SelectedGenres)
                {
                    var genre = await _context.Genres.FindAsync(int.Parse(genreId));
                    if (genre != null && !mediaToUpdate.Genres.Any(g => g.Id == genre.Id))
                    {
                        mediaToUpdate.Genres.Add(genre); // Add only if not already present
                    }
                }

                _context.Update(mediaToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaExists(viewModel.Media.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
                return RedirectToAction(nameof(Index));

      

return View(viewModel);
        }


        // Delete: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var media = await _context.Media.FindAsync(id);
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // Details: GET
        public async Task<IActionResult> Details(int id)
        {
            var media = await _context.Media
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }
    }

}
