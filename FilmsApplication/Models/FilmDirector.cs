using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class FilmDirector
{
    public int FilmDirectorId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    public int FilmId { get; set; }

    public int DirectorId { get; set; }

    public virtual Director Director { get; set; } = null!;

    [Display(Name = "Film")]
    public virtual Film Film { get; set; } = null!;
}
