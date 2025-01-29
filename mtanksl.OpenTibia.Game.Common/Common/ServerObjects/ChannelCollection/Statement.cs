using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Statement
    {
        public uint Id { get; set; }

        public int DatabasePlayerId { get; set; }

        public string Message { get; set; }

        public DateTime CreationDate { get; set; }
    }
}