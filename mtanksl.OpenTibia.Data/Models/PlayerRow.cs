namespace OpenTibia.Data
{
    public class PlayerRow
    {
        public string Name { get; set; }
              
        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }

        public int CoordinateZ { get; set; }

        public virtual WorldRow World { get; set; }
    }
}