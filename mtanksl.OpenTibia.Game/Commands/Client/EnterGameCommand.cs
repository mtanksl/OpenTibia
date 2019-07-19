using OpenTibia.Common.Objects;
using OpenTibia.Data;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class EnterGameCommand : Command
    {
        public EnterGameCommand(IConnection connection, EnterGameIncomingPacket packet)
        {
            Connection = connection;

            Packet = packet;
        }

        public IConnection Connection { get; set; }

        public EnterGameIncomingPacket Packet { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            Connection.Keys = Packet.Keys;

            if (Packet.TibiaDat != 1277983123 || Packet.TibiaPic != 1256571859 || Packet.TibiaSpr != 1277298068 || Packet.Version != 860)
            {
                context.Write(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );
            }
            else
            {
                var account = new PlayerRepository().GetAccount(Packet.Account, Packet.Password);

                if (account == null)
                {
                    context.Write(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );
                }
                else
                {
                    //Arrange



                    //Act



                    //Notify

                    List<Character> characters = new List<Character>();

                    foreach (var player in account.Players)
                    {
                        characters.Add( new Character(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
                    }

                    context.Write(Connection, new OpenSelectCharacterDialogOutgoingPacket(characters, (ushort)account.PremiumDays) );
                }
            }
        }
    }
}