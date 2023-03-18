using FilmsApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DbfilmsContext _context;

        public ChartController(DbfilmsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonDataCountry")]
        public JsonResult JsonDataCountry()
        {
            var countries = _context.Countries.Include(b => b.Actors).ToList();
            List<object> countryActor = new();
            countryActor.Add(new[] { "Country", "Number of actors" });
            foreach (var coutries in countries)
            {
                countryActor.Add(new object[] { coutries.CountryName, coutries.Actors.Count });
            }

            return new JsonResult(countryActor);
        }

        [HttpGet("JsonDataGenre")]
        public JsonResult JsonDataGenre()
        {
            var genres = _context.Genres.Include(m => m.FilmGenres).ToList();
            List<object> genreFilm = new();
            genreFilm.Add(new[] { "Genre", "Number of films:" });
            foreach (var genre in genres)
            {
                genreFilm.Add(new object[] { genre.GenreName, genre.FilmGenres.Count });
            }

            return new JsonResult(genreFilm);
        }
    }
}
