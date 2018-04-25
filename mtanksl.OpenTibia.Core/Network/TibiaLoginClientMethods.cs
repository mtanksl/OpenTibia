using System.Collections.Generic;
using OpenTibia.Data;

namespace OpenTibia
{
    public partial class TibiaLoginClient
    {
        private void EnterGame(EnterGameIncomingPacket packet)
        {
            Keys = packet.Keys;

            if (packet.TibiaDat != 1277983123 || packet.TibiaPic != 1256571859 || packet.TibiaSpr != 1277298068 || packet.Version != 860)
            {
                Response.Write( new SorryOutgoingPacket(true, "Only protocol 8.6 allowed.") );

                return;
            }

            var account = new PlayerRepository().GetAccount(packet.Account, packet.Password);

            if (account == null)
            {
                Response.Write( new SorryOutgoingPacket(true, "Account name or password is not correct.") );

                return;
            }

            List<Character> characters = new List<Character>();

            foreach (var player in account.Players)
            {
                characters.Add( new Character(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
            }

            Response.Write( new SelectCharacterOutgoingPacket(characters, (ushort)account.PremiumDays) );
        }
    }
}