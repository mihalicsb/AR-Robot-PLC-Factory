using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataServer.Models;

public partial class Factory
{
    public int Id { get; set; }

    [Display(Name = "Gyár név")] public string Name { get; set; } = null!;

    [Display(Name = "Szélesség")] public float Width { get; set; }

    [Display(Name = "Hossz")] public float Length { get; set; }

    public virtual ICollection<Robot> Robots { get; } = new List<Robot>();
}
