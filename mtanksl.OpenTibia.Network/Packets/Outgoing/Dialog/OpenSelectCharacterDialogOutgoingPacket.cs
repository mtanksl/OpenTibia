using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenSelectCharacterDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenSelectCharacterDialogOutgoingPacket(List<CharacterDto> characters, AccountStatus accountStatus, SubscriptionStatus subscriptionStatus, uint premiumDays)
        {
            this.Characters = characters;

            this.AccountStatus = accountStatus;

            this.SubscriptionStatus = subscriptionStatus;

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

        public AccountStatus AccountStatus { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }

        public uint PremiumDays { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x64 );

            if ( !features.HasFeatureFlag(FeatureFlag.GroupWorlds) )
            {
                writer.Write( (byte)Characters.Count );

                foreach (var character in Characters)
                {
                    writer.Write(character.PlayerName);

                    writer.Write(character.WorldName);

                    writer.Write( IPAddress.Parse(character.Ip) );

                    writer.Write(character.Port);

                    if (features.HasFeatureFlag(FeatureFlag.PreviewState) )
                    {
                        writer.Write(character.PreviewState);                    
                    }
                }
            }
            else
            {
                var worlds = Characters
                    .Select(c => new { WorldName = c.WorldName, Ip = c.Ip, Port = c.Port, PreviewState = c.PreviewState } )
                    .Distinct()
                    .Select( (w, i) => new { Index = (byte)i, World = w } )
                    .ToArray();

                writer.Write( (byte)worlds.Length);

                foreach (var world in worlds)
                {
                    writer.Write(world.Index);

                    writer.Write(world.World.WorldName);

                    writer.Write(world.World.Ip);

                    writer.Write(world.World.Port);

                    writer.Write(world.World.PreviewState);
                }

                writer.Write( (byte)Characters.Count );

                foreach (var character in Characters)
                {
                    writer.Write(worlds.Where(w => w.World.WorldName == character.WorldName &&
                                                   w.World.Ip == character.Ip &&
                                                   w.World.Port == character.Port &&
                                                   w.World.PreviewState == character.PreviewState).First().Index);

                    writer.Write(character.PlayerName);
                }
            }
                
            if ( !features.HasFeatureFlag(FeatureFlag.AccountStatus) )
            {
                writer.Write( (ushort)PremiumDays);
            }
            else
            {
                writer.Write( (byte)AccountStatus);

                writer.Write( (byte)SubscriptionStatus);

                writer.Write(PremiumDays);
            }
        }
    }
}