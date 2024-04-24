using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ExtPlayersInfoOutgoingPacket : IOutgoingPacket
    {      
        public ExtPlayersInfoOutgoingPacket(List<ExtPlayersInfoDto> players)
        {
            this.Players = players;
        }

        private List<ExtPlayersInfoDto> players;

        public List<ExtPlayersInfoDto> Players
        {
            get
            {
                return players ?? (players = new List<ExtPlayersInfoDto>() );
            }
            set
            {
                players = value;
            }
        }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x21);
            
            writer.Write( (uint)Players.Count );

            foreach (var player in Players)
            {
                writer.Write(player.Name);

                writer.Write(player.Level);
            }
        }
    }
}