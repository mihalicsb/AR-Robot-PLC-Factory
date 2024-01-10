using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataServer.Models;

public partial class PlcType
{
    public int Id { get; set; }

    [Display(Name = "Típús")] public string Name { get; set; } = null!;

    public virtual ICollection<Plc> Plcs { get; } = new List<Plc>();
}
