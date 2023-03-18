using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsApplication.Models;
using System.Threading;

namespace FilmsApplication.Controllers
{
    public class ActorsController : Controller
    {
        private readonly DbfilmsContext _context;

        public ActorsController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index(int? id, string? name)
        {
            var actors = _context.Actors;
            List<Actor> actorsFiltered;
            if (id != null && name != null)
            {
                actorsFiltered = await actors.Where(a => a.ActorCountryId == id).Include(a => a.ActorCountry).ToListAsync();
            }
            else
            {
                actorsFiltered = await actors.Include(a => a.ActorCountry).ToListAsync();
            }
            return View(actorsFiltered);
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .Include(a => a.ActorCountry)
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "FilmActors", new { id = actor.ActorId, name = actor.ActorName });
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            ViewData["ActorCountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,ActorName,ActorBirthDay,ActorDeathDay,ActorCountryId")] Actor actor)
        {

            if (ModelState.IsValid)
            {
                var existActorName = await _context.Actors.FirstOrDefaultAsync(c => c.ActorName == actor.ActorName);
                if (existActorName != null)
                {
                    ModelState.AddModelError("ActorName", "Not available name");
                    return View(existActorName);
                }

                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActorCountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", actor.ActorCountryId);
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            ViewData["ActorCountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", actor.ActorCountryId);
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,ActorName,ActorBirthDay,ActorDeathDay,ActorCountryId")] Actor actor)
        {
            if (id != actor.ActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ActorId))
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
            ViewData["ActorCountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", actor.ActorCountryId);
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .Include(a => a.ActorCountry)
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Actors == null)
            {
                return Problem("Entity set 'DbfilmsContext.Actors'  is null.");
            }
            //var actor = await _context.Actors.FindAsync(id);

            var actor = await _context.Actors
                .Include(m => m.FilmActors)
                .FirstOrDefaultAsync(m => m.ActorId == id);

            if (actor != null)
            {
                foreach (var m in actor.FilmActors)
                    _context.Remove(m);

                _context.Actors.Remove(actor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
          return (_context.Actors?.Any(e => e.ActorId == id)).GetValueOrDefault();
        }
    }
}
