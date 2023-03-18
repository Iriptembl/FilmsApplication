using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class FilmGenre
{
    public int FilmGenreId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    public int FilmId { get; set; }

    public int GenreId { get; set; }

    public virtual Film Film { get; set; } = null!;

    [Display(Name = "Film")]
    public virtual Genre Genre { get; set; } = null!;
}
