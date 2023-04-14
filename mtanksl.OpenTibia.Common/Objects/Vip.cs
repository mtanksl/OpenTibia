namespace OpenTibia.Common.Objects
{
    public class Vip
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + " Name: " + Name;
        }
    }
}