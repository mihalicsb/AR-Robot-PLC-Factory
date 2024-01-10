using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataServer.Models;

public partial class Plc
{
    public int Id { get; set; }

    [Display(Name = "Név")] public string Name { get; set; } = null!;

    [Display(Name = "Cím")] public string Address { get; set; } = null!;

    [Display(Name = "PLC típúsa")] public int TypeId { get; set; }

    [Display(Name = "DataBlock száma")] public int DbNumber { get; set; }

    public virtual ICollection<IoPort> IoPorts { get; } = new List<IoPort>();

    public virtual PlcType Type { get; set; } = null!;
}
