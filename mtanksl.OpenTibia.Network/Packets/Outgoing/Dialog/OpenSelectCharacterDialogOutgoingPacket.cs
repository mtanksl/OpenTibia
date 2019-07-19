using System.Net;
using System.Collections.Generic;
using OpenTibia.IO;
using OpenTibia.Common.Structures;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenSelectCharacterDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenSelectCharacterDialogOutgoingPacket(List<Character> characters, ushort premiumDays)
        {
            this.Characters = characters;

            this.PremiumDays = premiumDays;
        }

        private List<Character> characters;

        public List<Character> Characters
        {
            get
            {
                return characters ?? ( characters = new List<Character>() );
            }
            set
            {
                characters = value;
            }
        }

        public ushort PremiumDays { get; set; }


        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x64 );

            writer.Write( (byte)Characters.Count );

            foreach (var character in Characters)
            {
                writer.Write(character.Name);

                writer.Write(character.World);

                writer.Write( IPAddress.Parse(character.Ip) );

                writer.Write(character.Port);
            }

            writer.Write(PremiumDays);
        }
    }
}