using OpenTibia.Common.Objects;
using OpenTibia.IO;
using System.Collections.Generic;
using System.Net;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenSelectCharacterDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenSelectCharacterDialogOutgoingPacket(List<CharacterDto> characters, ushort premiumDays)
        {
            this.Characters = characters;

            this.PremiumDays = premiumDays;
        }

        private List<CharacterDto> characters;

        public List<CharacterDto> Characters
        {
            get
            {
                return characters ?? ( characters = new List<CharacterDto>() );
            }
            set
            {
                characters = value;
            }
        }

        public ushort PremiumDays { get; set; }


        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
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