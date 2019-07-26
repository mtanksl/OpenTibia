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

            //Stop walk

            server.CancelQueueForExecution(Constants.PlayerSchedulerEvent(Player) );

            //Stop follow and attack

            foreach (var creature in server.Map.GetCreatures() )
            {
                if (creature.FollowTarget == Player)
                {
                    creature.FollowTarget = null;
                }

                if (creature == Player)
                {
                    creature.FollowTarget = null;
                }
            }

            //Close channels

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

            //Close rule violations

            RuleViolation ruleViolation = server.RuleViolations.GetRuleViolationByReporter(Player);

            if (ruleViolation != null)
            {
                if (ruleViolation.Assignee == null)
                {
                    server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    foreach (var observer in server.Channels.GetChannel(3).GetPlayers() )
                    {
                        context.Write(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                    }
                }
                else
                {
                    server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    context.Write(ruleViolation.Assignee.Client.Connection, new CancelRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                }
            }

            ruleViolation = server.RuleViolations.GetRuleViolationByAssignee(Player);

            if (ruleViolation != null)
            {
                server.RuleViolations.RemoveRuleViolation(ruleViolation);

                context.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );
            }

            //Close containers

            foreach (var container in Player.Client.ContainerCollection.GetContainers() )
            {
                container.RemovePlayer(Player);
            }

            //Close windows

            foreach (var window in Player.Client.WindowCollection.GetWindows() )
            {
                window.RemovePlayer(Player);
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