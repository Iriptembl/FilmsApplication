using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsApplication.Models;
using System.Diagnostics.Metrics;
using System.Threading;

namespace FilmsApplication.Controllers
{
    public class FilmActorsController : Controller
    {
        private readonly DbfilmsContext _context;

        public FilmActorsController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: FilmActors
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if(id == null || name == null)
            {
                return RedirectToAction("Index", "Actors");
            }
            ViewBag.ActorId = id;
            ViewBag.ActorName = name;
            var dbfilmsContext = _context.FilmActors.Where(fa => fa.ActorId == id).Include(f => f.Actor).Include(f => f.Film);
            return View(await dbfilmsContext.ToListAsync());
        }

        // GET: FilmActors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FilmActors == null)
            {
                return NotFound();
            }

            var filmActor = await _context.FilmActors
                .Include(f => f.Actor)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.FilmActorId == id);
            if (filmActor == null)
            {
                return NotFound();
            }

            return View(filmActor);
        }

        // GET: FilmActors/Create
        public IActionResult Create(int id)
        {
            ViewBag.ActorId = id;
            ViewBag.ActorName = _context.Actors.Where(a => a.ActorId == id).FirstOrDefault().ActorName;
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmName");
            return View();
        }

        // POST: FilmActors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ActorId, [Bind("FilmActorId,FilmId")] FilmActor filmActor)
        {
            filmActor.ActorId = ActorId;
            if (ModelState.IsValid)
            {
                _context.Add(filmActor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmName", ViewBag.ActorName);
            return View(filmActor);
        }

        // GET: FilmActors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FilmActors == null)
            {
                return NotFound();
            }

            var filmActor = await _context.FilmActors.FindAsync(id);
            if (filmActor == null)
            {
                return NotFound();
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "ActorId", filmActor.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmActor.FilmId);
            return View(filmActor);
        }

        // POST: FilmActors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmActorId,FilmId,ActorId")] FilmActor filmActor)
        {
            if (id != filmActor.FilmActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmActor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmActorExists(filmActor.FilmActorId))
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
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "ActorId", filmActor.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmId", filmActor.FilmId);
            return View(filmActor);
        }

        // GET: FilmActors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FilmActors == null)
            {
                return NotFound();
            }

            var filmActor = await _context.FilmActors
                .Include(f => f.Actor)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.FilmActorId == id);
            if (filmActor == null)
            {
                return NotFound();
            }

            return View(filmActor);
        }

        // POST: FilmActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FilmActors == null)
            {
                return Problem("Entity set 'DbfilmsContext.FilmActors'  is null.");
            }
            var filmActor = await _context.FilmActors.FindAsync(id);
            if (filmActor != null)
            {
                _context.FilmActors.Remove(filmActor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmActorExists(int id)
        {
          return (_context.FilmActors?.Any(e => e.FilmActorId == id)).GetValueOrDefault();
        }
    }
}
