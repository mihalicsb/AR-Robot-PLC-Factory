using System.ComponentModel.DataAnnotations;

namespace ManagementWebServer.Models;

public partial class RobotType
{
    public int Id { get; set; }

    [Display(Name = "Robot típusa")] public string Name { get; set; } = null!;

    public virtual ICollection<Robot> Robots { get; } = new List<Robot>();
}
