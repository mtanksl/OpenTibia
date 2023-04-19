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
            List<SelectOutfit> outfits = new List<SelectOutfit>();

            switch (Player.Gender)
            {
                case Gender.Male:

                    outfits.Add(new SelectOutfit(Outfit.MaleCitizen.Id, "Citizen", Addon.None) );

                    outfits.Add(new SelectOutfit(Outfit.MaleHunter.Id, "Hunter", Addon.None) );

                    outfits.Add(new SelectOutfit(Outfit.MaleMage.Id, "Mage", Addon.None) );

                    outfits.Add(new SelectOutfit(Outfit.MaleKnight.Id, "Knight", Addon.None) );

                    break;

                case Gender.Female:

                    outfits.Add( new SelectOutfit(Outfit.FemaleCitizen.Id, "Citizen", Addon.None) );

                    outfits.Add( new SelectOutfit(Outfit.FemaleHunter.Id, "Hunter", Addon.None) );

                    outfits.Add( new SelectOutfit(Outfit.FemaleMage.Id, "Mage", Addon.None) );

                    outfits.Add( new SelectOutfit(Outfit.FemaleKnight.Id, "Knight", Addon.None) );

                    break;

                default:

                    throw new NotImplementedException();
            }

            if (Player.Vocation == Vocation.Gamemaster)
            {
                outfits.Add(new SelectOutfit(Outfit.GamemasterRed.Id, "Red", Addon.None) );

                outfits.Add(new SelectOutfit(Outfit.GamemasterGreen.Id, "Green", Addon.None) );

                outfits.Add(new SelectOutfit(Outfit.GamemasterBlue.Id, "Blue", Addon.None) );
            }

            Context.AddPacket(Player.Client.Connection, new OpenSelectOutfitDialogOutgoingPacket(Player.Outfit, outfits) );

            return Promise.Completed;
        }
    }
}