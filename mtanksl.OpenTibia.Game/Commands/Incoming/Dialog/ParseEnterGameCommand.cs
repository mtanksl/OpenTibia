using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseEnterGameCommand : Command
    {
        public ParseEnterGameCommand(IConnection connection, EnterGameIncomingPacket packet)
        {
            Connection = connection;

            Packet = packet;
        }

        public IConnection Connection { get; set; }

        public EnterGameIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            Connection.Keys = Packet.Keys;

            if (Packet.TibiaDat != 1277983123 || Packet.TibiaPic != 1256571859 || Packet.TibiaSpr != 1277298068 || Packet.Version != 860)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbAccount databaseAccount = Context.DatabaseContext.PlayerRepository.GetAccount(Packet.Account, Packet.Password);

            if (databaseAccount == null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            List<Character> characters = new List<Character>();

            foreach (var player in databaseAccount.Players)
            {
                characters.Add( new Character(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
            }

            Context.AddPacket(Connection, new OpenSelectCharacterDialogOutgoingPacket(characters, (ushort)databaseAccount.PremiumDays) );

            Context.Disconnect(Connection);

            return Promise.Completed;
        }
    }
}