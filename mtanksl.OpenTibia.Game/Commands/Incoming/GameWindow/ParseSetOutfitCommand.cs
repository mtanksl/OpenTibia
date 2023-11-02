using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseSetOutfitCommand : Command
    {
        public ParseSetOutfitCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            List<OutfitDto> outfits = new List<OutfitDto>();

            switch (Player.Gender)
            {
                case Gender.Male:

                    outfits.Add(new OutfitDto(Outfit.MaleCitizen.Id, "Citizen", Addon.None) );

                    outfits.Add(new OutfitDto(Outfit.MaleHunter.Id, "Hunter", Addon.None) );

                    outfits.Add(new OutfitDto(Outfit.MaleMage.Id, "Mage", Addon.None) );

                    outfits.Add(new OutfitDto(Outfit.MaleKnight.Id, "Knight", Addon.None) );

                    break;

                case Gender.Female:

                    outfits.Add( new OutfitDto(Outfit.FemaleCitizen.Id, "Citizen", Addon.None) );

                    outfits.Add( new OutfitDto(Outfit.FemaleHunter.Id, "Hunter", Addon.None) );

                    outfits.Add( new OutfitDto(Outfit.FemaleMage.Id, "Mage", Addon.None) );

                    outfits.Add( new OutfitDto(Outfit.FemaleKnight.Id, "Knight", Addon.None) );

                    break;

                default:

                    throw new NotImplementedException();
            }

            if (Player.Rank == Rank.Gamemaster)
            {
                outfits.Add(new OutfitDto(Outfit.GamemasterBlue.Id, "Gamemaster", Addon.None) );

                outfits.Add(new OutfitDto(Outfit.GamemasterRed.Id, "Customer Support", Addon.None) );

                outfits.Add(new OutfitDto(Outfit.GamemasterGreen.Id, "Community Manager", Addon.None) );
            }

            Context.AddPacket(Player.Client.Connection, new OpenSelectOutfitDialogOutgoingPacket(Player.BaseOutfit, outfits) );

            return Promise.Completed;
        }
    }
}