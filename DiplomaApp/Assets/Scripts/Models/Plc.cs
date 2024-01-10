using System.Collections.Generic;

public class Plc
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string Type { get; set; }
    public int DbNumber { get; set; }
    public string Name { get; set; }
    public List<IO> IOs { get; set; }
}

public class IO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Offset { get; set; }
    public int Bit { get; set; }
    public int Direction { get; set; }
    public int Value { get; set; }
}
