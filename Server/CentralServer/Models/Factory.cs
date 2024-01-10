using System.ComponentModel.DataAnnotations;

namespace ManagementWebServer.Models;

public partial class Factory
{
    public int Id { get; set; }

    [Display(Name = "Gyár neve")] public string Name { get; set; } = null!;

    [Display(Name = "Gyár szélessége")] public float Width { get; set; }

    [Display(Name = "Gyár hosszúsága")] public float Length { get; set; }

    public virtual ICollection<Robot> Robots { get; } = new List<Robot>();
}
