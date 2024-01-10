using System.ComponentModel.DataAnnotations.Schema;

namespace DataServer.Models;

public partial class Robot
{
    [NotMapped]
    public List<RobotTask> RobotTasks { get; set; } = new List<RobotTask>();

    public sealed class RobotTask
    {
        public string Name { get; set; } = default!;
        public Vector3 Position { get; set; } = default!;

        public sealed class Vector3
        {
            private float x, y, z;
            public float X
            {
                get { return x / 1000; }
                set { x = value; }
            }
            public float Y
            {
                get { return y / 1000; }
                set { y = value; }
            }
            public float Z
            {
                get { return z / 1000; }
                set { z = value; }
            }
        }
    }

}

