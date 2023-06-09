﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class Director
{
    public int DirectorId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Name")]
    public string DirectorName { get; set; } = null!;

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Date of birth")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DirectorBirthDay { get; set; }

    [Display(Name = "Date of death")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? DirectorDeathDay { get; set; }

    public virtual ICollection<FilmDirector> FilmDirectors { get; } = new List<FilmDirector>();
}
