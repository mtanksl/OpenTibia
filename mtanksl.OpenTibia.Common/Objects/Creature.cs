using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Common.Objects
{
    public abstract class Creature : IContent
    {
        public TopOrder TopOrder
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IContainer Container
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public uint Id { get; set; }

        public string Name { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public Direction Direction { get; set; }

        public Light Light { get; set; }

        public Outfit Outfit { get; set; }

        public ushort Speed { get; set; }

        public int DiagonalDelay { get; set; }

        public SkullIcon SkullIcon { get; set; }

        public PartyIcon PartyIcon { get; set; }

        public WarIcon WarIcon { get; set; }

        public bool Block { get; set; }
    }
}