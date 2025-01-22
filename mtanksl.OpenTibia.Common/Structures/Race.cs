using System.Xml.Serialization;

namespace OpenTibia.Common.Structures
{
    public enum Race : byte
    {
        [XmlEnum("blood")]
        Blood = 0,

        [XmlEnum("energy")]
        Energy = 1,

        [XmlEnum("fire")]
        Fire = 2,

        [XmlEnum("venom")]
        Venom = 3,

        [XmlEnum("undead")]
        Undead = 4
    }
}