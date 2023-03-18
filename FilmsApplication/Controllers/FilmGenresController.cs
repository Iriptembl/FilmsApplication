using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsApplication.Models;

namespace FilmsApplication.Controllers
{
    public class FilmGenresController : Controller
    {
        private readonly DbfilmsContext _context;

        public FilmGenresController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: FilmGenres
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null || name == null)
            {
                return RedirectToAction("Index", "Genres");
            }
            ViewBag.GenreId = id;
            ViewBag.GenreName = name;
            var dbfilmsContext = _context.FilmGenres.Where(fg => fg.GenreId == id).Include(f => f.Film).Include(f => f.Genre);
            return View(await dbfilmsContext.ToListAsync());
        }

        // GET: FilmGenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FilmGenres == null)
            {
                return NotFound();
            }

            var filmGenre = await _context.FilmGenres
                .Include(f => f.Film)
                .Include(f => f.Genre)
                .FirstOrDefaultAsync(m => m.FilmGenreId == id);
            if (filmGenre == null)
            {
                return NotFound();
            }

            return View(filmGenre);
        }

        // GET: FilmGenres/Create
        public IActionResult Create(int id)
        {
            ViewBag.GenreId = id;
            ViewBag.GenreName = _context.Genres.Where(g => g.GenreId == id).FirstOrDefault().GenreName;
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmName");
            return View();
        }

        // POST: FilmGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int GenreId, [Bind("FilmGenreId,FilmId")] FilmGenre filmGenre)
        {
            filmGenre.GenreId = GenreId;

            if (ModelState.IsValid)
            {
                _context.Add(filmGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmGenre.FilmId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", filmGenre.GenreId);
            return View(filmGenre);
        }

        // GET: FilmGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FilmGenres == null)
            {
                return NotFound();
            }

            var filmGenre = await _context.FilmGenres.FindAsync(id);
            if (filmGenre == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmGenre.FilmId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", filmGenre.GenreId);
            return View(filmGenre);
        }

        // POST: FilmGenres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmGenreId,FilmId,GenreId")] FilmGenre filmGenre)
        {
            if (id != filmGenre.FilmGenreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmGenreExists(filmGenre.FilmGenreId))
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
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmGenre.FilmId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", filmGenre.GenreId);
            return View(filmGenre);
        }

        // GET: FilmGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FilmGenres == null)
            {
                return NotFound();
            }

            var filmGenre = await _context.FilmGenres
                .Include(f => f.Film)
                .Include(f => f.Genre)
                .FirstOrDefaultAsync(m => m.FilmGenreId == id);
            if (filmGenre == null)
            {
                return NotFound();
            }

            return View(filmGenre);
        }

        // POST: FilmGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FilmGenres == null)
            {
                return Problem("Entity set 'DbfilmsContext.FilmGenres'  is null.");
            }
            var filmGenre = await _context.FilmGenres.FindAsync(id);
            if (filmGenre != null)
            {
                _context.FilmGenres.Remove(filmGenre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmGenreExists(int id)
        {
          return (_context.FilmGenres?.Any(e => e.FilmGenreId == id)).GetValueOrDefault();
        }
    }
}
