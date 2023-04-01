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
    public class FilmsController : Controller
    {
        private readonly DbfilmsContext _context;

        public FilmsController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
              return _context.Films != null ? 
                          View(await _context.Films.ToListAsync()) :
                          Problem("Entity set 'DbfilmsContext.Films'  is null.");
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.FilmId == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,FilmName,FilmDateRelease,FilmRating")] Film film)
        {
            if (ModelState.IsValid)
            {

                DateTime enteredDate = film.FilmDateRelease;
                
                DateTime startDate = new DateTime(1960, 1, 1);
                DateTime endDate = new DateTime(2027, 1, 1);

                if (enteredDate <= startDate || enteredDate >= endDate)
                {
                    ModelState.AddModelError("FilmDateRelease", "Not available value. Set in range: 01.01.1960 - today");
                    return View(film);
                }

                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,FilmName,FilmDateRelease,FilmRating")] Film film)
        {
            if (id != film.FilmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existFilmName = await _context.Films.FirstOrDefaultAsync(c => c.FilmId != film.FilmId && c.FilmName == film.FilmName);

                if (existFilmName != null)
                {
                    ModelState.AddModelError("FilmName", "Not available name of film");
                    return View(existFilmName);
                }
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.FilmId))
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
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.FilmId == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Films == null)
            {
                return Problem("Entity set 'DbfilmsContext.Films'  is null.");
            }

            var film = await _context.Films
                .Include(m => m.FilmGenres)
                .Include(m => m.FilmActors)
                .Include(m => m.FilmDirectors)
                .FirstOrDefaultAsync(m => m.FilmId == id);

            if (film != null)
            {
                foreach (var m in film.FilmGenres)
                    _context.Remove(m);

                foreach (var m in film.FilmActors)
                    _context.Remove(m);

                foreach (var m in film.FilmDirectors)
                    _context.Remove(m);

                _context.Films.Remove(film);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
          return (_context.Films?.Any(e => e.FilmId == id)).GetValueOrDefault();
        }
    }
}
