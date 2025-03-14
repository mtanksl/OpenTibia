﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkSouthIncomingPacket : IIncomingPacket
    {
        public MoveDirection MoveDirection
        {
            get
            {
                return MoveDirection.South;
            }
        }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {

        }
    }
}