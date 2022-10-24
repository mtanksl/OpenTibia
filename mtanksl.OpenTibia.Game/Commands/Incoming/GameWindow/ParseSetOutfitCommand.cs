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
                
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                List<SelectOutfit> outfits;

                switch (Player.Gender)
                {
                    case Gender.Male:

                        outfits = new List<SelectOutfit>()
                        {
                            new SelectOutfit(128, "Citizen", Addon.None),

                            new SelectOutfit(129, "Hunter", Addon.None),

                            new SelectOutfit(130, "Mage", Addon.None),

                            new SelectOutfit(131, "Knight", Addon.None)
                        };

                        break;

                    case Gender.Female:

                        outfits = new List<SelectOutfit>()
                        {
                            new SelectOutfit(136, "Citizen", Addon.None),

                            new SelectOutfit(137, "Hunter", Addon.None),

                            new SelectOutfit(138, "Mage", Addon.None),

                            new SelectOutfit(139, "Knight", Addon.None)
                        };

                        break;

                    default:

                        throw new NotImplementedException();
                }

                context.AddPacket(Player.Client.Connection, new OpenSelectOutfitDialogOutgoingPacket(Player.Outfit, outfits) );

                resolve(context);
            } );
        }
    }
}