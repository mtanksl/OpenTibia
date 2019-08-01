using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class LookCommand : Command
    {
        public LookCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected void LookItem(Item item, Server server, Context context)
        {
            //Arrange

            //Act

            //Notify

            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see nothing special.") );

            base.Execute(server, context);
        }

        protected void LookCreature(Creature creature, Server server, Context context)
        {
            //Arrange

            //Act

            //Notify

            if (Player == creature)
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see yourself.") );
            }
            else
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see " + creature.Name + ".") );
            }

            base.Execute(server, context);
        }
    }
}