namespace OpenTibia.FileFormats.Xml.Monsters
{
    // This format does not make much sense. Only need 1 node with multiple attributes.

    public class ImmunityItem
    {
        public int? Physical { get; set; }

        public int? Earth { get; set; }

        public int? Fire { get; set; }

        public int? Energy { get; set; }

        public int? Ice { get; set; }

        public int? Death { get; set; }

        public int? Holy { get; set; }

        public int? Drown { get; set; }

        public int? ManaDrain { get; set; }

        public int? LifeDrain { get; set; }
    }
}