using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class SelectOutfitCommand : Command
    {
        private Server server;

        public SelectOutfitCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }
                
        public override void Execute(Context context)
        {
            //Arrange

            List<SelectOutfit> outfits = new List<SelectOutfit>()
            {
                new SelectOutfit(128, "Citizen", Addon.None),

                new SelectOutfit(129, "Hunter", Addon.None),

                new SelectOutfit(130, "Mage", Addon.None),

                new SelectOutfit(131, "Knight", Addon.None)
            };

            //Act

            //Notify

            context.Response.Write(Player.Client.Connection, new OpenSelectOutfitDialog(Player.Outfit, outfits) );
        }
    }
}