using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseSetOutfitCommand : IncomingCommand
    {
        public ParseSetOutfitCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            List<OutfitDto> outfits = new List<OutfitDto>();

            foreach (var pair in Player.Outfits.GetIndexed() )
            {
                OutfitConfig outfitConfig = Context.Server.Outfits.GetOutfitById(pair.Key);

                if (outfitConfig != null)
                {
                    outfits.Add(new OutfitDto(pair.Key, outfitConfig.Name, pair.Value) );
                }
                else
                {
                    outfits.Add(new OutfitDto(pair.Key, "Unknown", pair.Value) );
                }
            }

            if (Player.Rank == Rank.Gamemaster)
            {
                outfits.Add(new OutfitDto(Outfit.GamemasterBlue.Id, "Gamemaster", Addon.None) );

                outfits.Add(new OutfitDto(Outfit.GamemasterRed.Id, "Customer Support", Addon.None) );

                outfits.Add(new OutfitDto(Outfit.GamemasterGreen.Id, "Community Manager", Addon.None) );
            }

            Context.AddPacket(Player, new OpenSelectOutfitDialogOutgoingPacket(Player.BaseOutfit, outfits) );

            return Promise.Completed;
        }
    }
}