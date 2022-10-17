using OpenTibia.Common.Objects;
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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Connection.Keys = Packet.Keys;

                if (Packet.TibiaDat != 1277983123 || Packet.TibiaPic != 1256571859 || Packet.TibiaSpr != 1277298068 || Packet.Version != 860)
                {
                    context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                    context.Disconnect(Connection);

                    resolve(context);
                }
                else
                {
                    var databaseAccount = context.DatabaseContext.PlayerRepository.GetAccount(Packet.Account, Packet.Password);

                    if (databaseAccount == null)
                    {
                        context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                        context.Disconnect(Connection);

                        resolve(context);
                    }
                    else
                    {
                        List<Character> characters = new List<Character>();

                        foreach (var player in databaseAccount.Players)
                        {
                            characters.Add( new Character(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
                        }

                        context.AddPacket(Connection, new OpenSelectCharacterDialogOutgoingPacket(characters, (ushort)databaseAccount.PremiumDays) );

                        context.Disconnect(Connection);

                        resolve(context);
                    }
                }
            } );            
        }
    }
}