using System;
using System.Collections.Generic;

namespace DataServer.Models;

public partial class RobotType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Robot> Robots { get; } = new List<Robot>();
}
