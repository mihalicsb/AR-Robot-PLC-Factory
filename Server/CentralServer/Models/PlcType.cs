using System.ComponentModel.DataAnnotations;

namespace ManagementWebServer.Models;

public partial class PlcType
{
    public int Id { get; set; }

    [Display(Name = "PLC típusa")] public string Name { get; set; } = null!;

    public virtual ICollection<Plc> Plcs { get; } = new List<Plc>();
}
