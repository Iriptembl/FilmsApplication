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
    public class FilmDirectorsController : Controller
    {
        private readonly DbfilmsContext _context;

        public FilmDirectorsController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: FilmDirectors
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null || name == null)
            {
                return RedirectToAction("Index", "Directors");
            }
            ViewBag.DirectorId = id;
            ViewBag.DirectorName = name;
            var dbfilmsContext = _context.FilmDirectors.Where(fd => fd.DirectorId == id).Include(f => f.Director).Include(f => f.Film);
            return View(await dbfilmsContext.ToListAsync());
        }

        // GET: FilmDirectors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FilmDirectors == null)
            {
                return NotFound();
            }

            var filmDirector = await _context.FilmDirectors
                .Include(f => f.Director)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.FilmDirectorId == id);
            if (filmDirector == null)
            {
                return NotFound();
            }

            return View(filmDirector);
        }

        // GET: FilmDirectors/Create
        public IActionResult Create(int id)
        {
            ViewBag.DirectorId = id;
            ViewBag.DirectorName = _context.Directors.Where(d => d.DirectorId == id).FirstOrDefault().DirectorName;
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmName");
            return View();
        }

        // POST: FilmDirectors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int DirectorId, [Bind("FilmDirectorId,FilmId")] FilmDirector filmDirector)
        {
            filmDirector.DirectorId = DirectorId;

            if (ModelState.IsValid)
            {
                _context.Add(filmDirector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "DirectorId", "DirectorId", filmDirector.DirectorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmDirector.FilmId);
            return View(filmDirector);
        }

        // GET: FilmDirectors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FilmDirectors == null)
            {
                return NotFound();
            }

            var filmDirector = await _context.FilmDirectors.FindAsync(id);
            if (filmDirector == null)
            {
                return NotFound();
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "DirectorId", "DirectorId", filmDirector.DirectorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmDirector.FilmId);
            return View(filmDirector);
        }

        // POST: FilmDirectors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmDirectorId,FilmId,DirectorId")] FilmDirector filmDirector)
        {
            if (id != filmDirector.FilmDirectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmDirector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmDirectorExists(filmDirector.FilmDirectorId))
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
            ViewData["DirectorId"] = new SelectList(_context.Directors, "DirectorId", "DirectorId", filmDirector.DirectorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmDirector.FilmId);
            return View(filmDirector);
        }

        // GET: FilmDirectors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FilmDirectors == null)
            {
                return NotFound();
            }

            var filmDirector = await _context.FilmDirectors
                .Include(f => f.Director)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.FilmDirectorId == id);
            if (filmDirector == null)
            {
                return NotFound();
            }

            return View(filmDirector);
        }

        // POST: FilmDirectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FilmDirectors == null)
            {
                return Problem("Entity set 'DbfilmsContext.FilmDirectors'  is null.");
            }
            var filmDirector = await _context.FilmDirectors.FindAsync(id);
            if (filmDirector != null)
            {
                _context.FilmDirectors.Remove(filmDirector);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmDirectorExists(int id)
        {
          return (_context.FilmDirectors?.Any(e => e.FilmDirectorId == id)).GetValueOrDefault();
        }
    }
}
