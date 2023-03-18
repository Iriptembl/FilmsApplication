using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Genre")]
    public string GenreName { get; set; } = null!;

    public virtual ICollection<FilmGenre> FilmGenres { get; } = new List<FilmGenre>();
}
