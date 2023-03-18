using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsApplication.Models;

public partial class Country
{
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Can not be empty.")]
    [Display(Name = "Country")]
    public string CountryName { get; set; } = null!;

    public virtual ICollection<Actor> Actors { get; } = new List<Actor>();
}
