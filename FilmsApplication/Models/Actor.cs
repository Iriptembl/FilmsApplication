using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class Actor
{
    public int ActorId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Name")]
    public string ActorName { get; set; } = null!;

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Date of birth")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime ActorBirthDay { get; set; }

    [Display(Name = "Date of death")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? ActorDeathDay { get; set; }
    [Display(Name = "Country")]
    public int ActorCountryId { get; set; }
    [Display(Name = "Country")]

    public virtual Country ActorCountry { get; set; } = null!;

    public virtual ICollection<FilmActor> FilmActors { get; } = new List<FilmActor>();
}
