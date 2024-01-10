using System.ComponentModel.DataAnnotations;

namespace ManagementWebServer.Models;

public partial class Plc
{
    public int Id { get; set; }

    [Display(Name = "PLC neve")] public string Name { get; set; } = null!;

    [Display(Name = "Cím/URL")] public string Address { get; set; } = null!;

    [Display(Name = "PLC típusa ID (PLC model)")] public int TypeId { get; set; }

    [Display(Name = "Adatblokk száma")] public int DbNumber { get; set; }

    public virtual ICollection<IoPort> IoPorts { get; } = new List<IoPort>();

    public virtual PlcType Type { get; set; } = null!;
}
