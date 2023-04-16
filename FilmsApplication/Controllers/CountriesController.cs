using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsApplication.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ClosedXML.Excel;
using System.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FilmsApplication.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DbfilmsContext _context;

        public CountriesController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return _context.Countries != null ?
                        View(await _context.Countries.ToListAsync()) :
                        Problem("Entity set 'DbfilmsContext.Countries'  is null.");
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Actors", new { id = country.CountryId, name = country.CountryName });
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryId,CountryName")] Country country)
        {
            if (ModelState.IsValid)
            {
                var existContryName = await _context.Countries.FirstOrDefaultAsync(c => c.CountryName == country.CountryName);
                if (existContryName != null)
                {
                    ModelState.AddModelError("CountryName", "Not available name");
                    return View(existContryName);
                }

                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CountryId,CountryName")] Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryId))
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
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'DbfilmsContext.Countries'  is null.");
            }
            var country = await _context.Countries
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.CountryId == id);

            if (country != null)
            {
                foreach (var m in country.Actors)
                    _context.Remove(m);

                _context.Countries.Remove(country);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.CountryId == id)).GetValueOrDefault();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workbook = new XLWorkbook(stream))
                        {
                            foreach (IXLWorksheet worksheet in workbook.Worksheets)
                            {
                                Country newcountry;
                                var c = (from ctry in _context.Countries
                                         where ctry.CountryName.Contains(worksheet.Name)
                                         select ctry).ToList();

                                if (c.Count > 0)
                                {
                                    newcountry = c[0];
                                }
                                else
                                {
                                    newcountry = new Country();
                                    newcountry.CountryName = worksheet.Name;
                                    _context.Countries.Add(newcountry);
                                }
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Actor actor;
                                        var ac = (from dep in _context.Actors where dep.ActorName.Contains(row.Cell(1).Value.ToString()) select dep).ToList();

                                        if (ac.Count == 0)
                                        {
                                            actor = new Actor();
                                            actor.ActorName = row.Cell(1).Value.ToString();
                                            actor.ActorBirthDay = row.Cell(2).Value;
                                            if (row.Cell(3).Value.ToString() == "")
                                            {
                                                actor.ActorDeathDay = null;
                                            }
                                            else
                                            {
                                                actor.ActorDeathDay = row.Cell(3).Value;
                                            }
                                            actor.ActorCountry = newcountry;
                                            _context.Actors.Add(actor);
                                        }
                                        else actor = ac[0];

                                        for (int i = 4; i < 100; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Film film;

                                                var f = (from fil in _context.Films
                                                         where fil.FilmName.Contains(row.Cell(i).Value.ToString())
                                                         select fil).ToList();
                                                if (f.Count > 0)
                                                {
                                                    film = f[0];
                                                }
                                                else
                                                {
                                                    film = new Film();
                                                    film.FilmName = row.Cell(i).Value.ToString();
                                                    film.FilmDateRelease = DateTime.Now;
                                                    film.FilmRating = 0;
                                                    _context.Add(film);
                                                }
                                                FilmActor fa = new FilmActor();
                                                var fActor = (from filmActor in _context.FilmActors
                                                              where filmActor.Actor == actor
                                                              where filmActor.Film == film
                                                              select filmActor).FirstOrDefault();
                                                if (fActor is null)
                                                {
                                                    fa.Actor = actor;
                                                    fa.Film = film;
                                                    _context.FilmActors.Add(fa);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var countries = _context.Countries.Include("Actors").ToList();

                foreach (var c in countries)
                {
                    var worksheet = workbook.Worksheets.Add(c.CountryName);

                    worksheet.Cell("A1").Value = "Name";
                    worksheet.Cell("B1").Value = "Date of birth";
                    worksheet.Cell("C1").Value = "Date of death";
                    worksheet.Cell("D1").Value = "Film 1";
                    worksheet.Cell("E1").Value = "Film 2";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var actors = c.Actors.ToList();

                    for (int i = 0; i < actors.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = actors[i].ActorName;
                        worksheet.Cell(i + 2, 2).Value = actors[i].ActorBirthDay;
                        worksheet.Cell(i + 2, 3).Value = actors[i].ActorDeathDay;

                        var fa = _context.FilmActors.Where(a => a.ActorId == actors[i].ActorId).Include("Film").ToList();

                        int j = 0;
                        foreach (var a in fa)
                        {
                            if (j < 2)
                            {
                                worksheet.Cell(i + 2, j + 4).Value = a.Film.FilmName;
                                j++;
                            }
                        }
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"actors_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };

                }
            }
        }



    }  
}