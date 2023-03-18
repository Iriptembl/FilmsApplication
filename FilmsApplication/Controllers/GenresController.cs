using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsApplication.Models;
using System.Numerics;

namespace FilmsApplication.Controllers
{
    public class GenresController : Controller
    {
        private readonly DbfilmsContext _context;

        public GenresController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
              return _context.Genres != null ? 
                          View(await _context.Genres.ToListAsync()) :
                          Problem("Entity set 'DbfilmsContext.Genres'  is null.");
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "FilmGenres", new { id = genre.GenreId, name = genre.GenreName });
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GenreId,GenreName")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenreId,GenreName")] Genre genre)
        {
            if (id != genre.GenreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existGenreName = await _context.Genres.FirstOrDefaultAsync(c => c.GenreName == genre.GenreName);
                if (existGenreName != null)
                {
                    ModelState.AddModelError("GenreName", "Not available name");
                    return View(existGenreName);
                }
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.GenreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'DbfilmsContext.Genres'  is null.");
            }
            //var genre = await _context.Genres.FindAsync(id);


            var genre = await _context.Genres
                .Include(m => m.FilmGenres)
                .FirstOrDefaultAsync(m => m.GenreId == id);

            if (genre != null)
            {
                foreach (var m in genre.FilmGenres)
                    _context.Remove(m);

                _context.Genres.Remove(genre);
            }    
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
          return (_context.Genres?.Any(e => e.GenreId == id)).GetValueOrDefault();
        }

        // Genre/Confrim
        public async Task<IActionResult> ConfirmGenre(int genreId, int filmId)
        {
            var FilmGenre = _context.FilmGenres.Where(b => b.FilmId == filmId).Where(b => b.GenreId == genreId).FirstOrDefault();
            if (FilmGenre != null)
            {
                // You already add this genre
                return RedirectToAction("Index", "Genres", new { filmId });
            }
            var filmGenre = new FilmGenre
            {
                GenreId = genreId,
                FilmId = filmId
            };

            _context.FilmGenres.Add(filmGenre);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Genres", new { filmId });
        }
    }
}
    