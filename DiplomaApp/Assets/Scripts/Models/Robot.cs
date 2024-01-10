using System.Collections.Generic;

    public class Robot
    {
        public string Guid { get; set; }
        public string Address { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string HostName { get; set; }
        public string Version { get; set; }
        public float Xpos { get; set; }
        public float Ypos { get; set; }
        public float ZOrinetation { get; set; }
        public string FactoryName { get; set; }
        public float FactoryWidth { get; set; }
        public float FactoryLength { get; set; }
        public int Virtual { get; set; }
        public string Type { get; set; }
        public List<RobotTask> RobotTasks { get; set; }

        public string Details()
        {
            string details = "Robot adatok:\n-------------------\n";
            details += "GUID:\n" + Guid + "\n";
            details += "Álnév: " + Alias + "\n";
            details += "Név: " + Name + "\n";
            details += "Rendszer neve: "+ SystemName + "\n";
            details += "Kiszolgáló neve:\n" +HostName + "\n";
            details += "RobotWare Verzió: " + Version + "\n";
            details += "Virtuális robot: " +  (Virtual == 1 ? "igen" : "nem") + "\n";
            foreach(var task in RobotTasks)
            {
                details += "TASK neve: " + task.Name + "\n";
                details += "Program RobTarget pozíció:\n" + task.Position.X + ", " + task.Position.Y + ", " + task.Position.Z + "\n";
            }
            
            return details;
        }
    }
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class RobotTask
    {
        public string Name { get; set; }
        public Position Position { get; set; }
    }


