using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class FilmActor
{
    public int FilmActorId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    public int FilmId { get; set; }

    public int ActorId { get; set; }

    public virtual Actor Actor { get; set; } = null!;

    [Display(Name = "Film")]
    public virtual Film Film { get; set; } = null!;
}
