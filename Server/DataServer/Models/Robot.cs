using System;
using System.Collections.Generic;

namespace DataServer.Models;

public partial class Robot
{
    public string Guid { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public string? Address { get; set; }

    public string? Name { get; set; }

    public string? SystemName { get; set; }

    public string? HostName { get; set; }

    public string? Version { get; set; }

    /// <summary>
    /// 0 virtual, 1 real
    /// </summary>
    public sbyte? Virtual { get; set; }

    public float Xpos { get; set; }

    public float Ypos { get; set; }

    public float ZOrinetation { get; set; }

    public int TypeId { get; set; }

    public int FactoryId { get; set; }

    public virtual Factory Factory { get; set; } = null!;

    public virtual RobotType Type { get; set; } = null!;
}
