using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataServer.Models;

public partial class IoPort
{
    public int Id { get; set; }

    [Display(Name = "I/O Név")] public string Name { get; set; } = null!;

    public int PlcId { get; set; }

    /// <summary>
    /// 0 - In,   1-Out
    /// </summary>
    [Display(Name = "Irány")] public sbyte Direction { get; set; }

    [Display(Name = "Eltolás")] public int Offset { get; set; }

    [Display(Name = "Bit")] public int Bit { get; set; }

    [Display(Name = "Érték")] public int? Value { get; set; }

    public virtual Plc Plc { get; set; } = null!;
}
