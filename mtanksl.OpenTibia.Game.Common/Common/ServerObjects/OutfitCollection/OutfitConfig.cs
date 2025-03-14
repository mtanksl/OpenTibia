﻿using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class OutfitConfig
    {
        public ushort Group { get; set; }

        public ushort Id { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public bool Premium { get; set; }

        public bool AvailableAtOnce { get; set; }

        public int MinClientVersion { get; set; }
    }
}