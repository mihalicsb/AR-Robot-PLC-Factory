using System.ComponentModel.DataAnnotations;

namespace ManagementWebServer.Models;

public partial class Robot
{
    [Display(Name = "GUID")] public string Guid { get; set; } = null!;

    [Display(Name = "Álnév")] public string Alias { get; set; } = null!;

    [Display(Name = "Direkt keresési cím")] public string? Address { get; set; }

    [Display(Name = "Robot neve")] public string? Name { get; set; }

    [Display(Name = "Rendszernév")] public string? SystemName { get; set; }

    [Display(Name = "HOST név")] public string? HostName { get; set; }

    [Display(Name = "Verzió")] public string? Version { get; set; }

    /// <summary>
    /// 0 virtual, 1 real
    /// </summary>
    [Display(Name = "Virtuális")] public sbyte? Virtual { get; set; }

    [Display(Name = "Robot térbeli X pozíciója")] public float Xpos { get; set; }

    [Display(Name = "Robot térbeli Y pozíciója")] public float Ypos { get; set; }

    [Display(Name = "Robot térbeli Z irányultsága")] public float ZOrinetation { get; set; }

    public int TypeId { get; set; }

    public int FactoryId { get; set; }

    public virtual Factory Factory { get; set; } = null!;

    public virtual RobotType Type { get; set; } = null!;
}
