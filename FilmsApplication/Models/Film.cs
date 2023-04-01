using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class Film
{
    public int FilmId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Film")]
    public string FilmName { get; set; } = null!;

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Release date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime FilmDateRelease { get; set; }
    
    [Range(1, 10,
            ErrorMessage = "Must be from 1 to 10")]
    [Display(Name = "Rating")]
    public int? FilmRating { get; set; }

    public virtual ICollection<FilmActor> FilmActors { get; } = new List<FilmActor>();

    public virtual ICollection<FilmDirector> FilmDirectors { get; } = new List<FilmDirector>();

    public virtual ICollection<FilmGenre> FilmGenres { get; } = new List<FilmGenre>();
}
