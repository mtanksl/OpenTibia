using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        public LogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            Position fromPosition = fromTile.Position;

            //Act

            server.Map.RemoveCreature(Player);

            fromTile.RemoveContent(fromIndex);

            //Clear events...

            server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) );

            //Clear channels...

            foreach (var channel in server.Channels.GetChannels().ToList() )
            {
                if (channel.ContainsPlayer(Player) )
                {
                    channel.RemovePlayer(Player);
                }

                PrivateChannel privateChannel = channel as PrivateChannel;

                if (privateChannel != null)
                {
                    if (privateChannel.ContainsInvitation(Player) )
                    {
                        privateChannel.RemoveInvitation(Player);
                    }

                    if (privateChannel.Owner == Player)
                    {
                        foreach (var observer in privateChannel.GetPlayers().ToList() )
                        {
                            context.Write(observer.Client.Connection, new CloseChannelOutgoingPacket(channel.Id) );

                            privateChannel.RemovePlayer(observer);
                        }

                        foreach (var observer in privateChannel.GetInvitations().ToList() )
                        {
                            privateChannel.RemoveInvitation(observer);
                        }

                        server.Channels.RemoveChannel(privateChannel);
                    }
                }
            }

            //Clear rule violations...

            foreach (var ruleViolation in server.RuleViolations.GetRuleViolations().ToList() )
            {
                if (ruleViolation.Reporter == Player)
                {

                }

                if (ruleViolation.Assignee == Player)
                {

                }
            }
            
            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    if (observer.Tile.Position.CanSee(fromPosition) )
                    {
                        context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex),

                                                                  new ShowMagicEffectOutgoingPacket(fromPosition, MagicEffectType.Puff) );
                    }
                }
            }

            context.Disconnect(Player.Client.Connection);

            base.Execute(server, context);
        }
    }
}