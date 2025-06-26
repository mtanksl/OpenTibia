using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkChannelYellowCommand : IncomingCommand
    {
        public ParseTalkChannelYellowCommand(Player player, ushort channelId, string message)
        {
            Player = player;

            ChannelId = channelId;

            Message = message;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(Player);

            if (playerIdleBehaviour != null)
            {
                playerIdleBehaviour.SetLastAction();
            }

            if (Player.Level == 1)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouMayNotSpeakIntoChannelsAsLongAsYouAreOnLevel1) );

                return Promise.Break;
            }

            PlayerMuteBehaviour playerChannelMuteBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerMuteBehaviour>(Player);

            if (playerChannelMuteBehaviour != null)
            {
                string message;

                if (playerChannelMuteBehaviour.IsMuted(out message) )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, message) );

                    return Promise.Break;
                }      
            }

            Channel channel = Context.Server.Channels.GetChannel(ChannelId);

            if (channel != null)
            {
                if (channel.ContainerMember(Player) )
                {
                    if (channel.Flags.Is(ChannelFlags.Guild) )
                    {
                        Guild guild = Context.Server.Guilds.GetGuildThatContainsMember(Player);

                        if (guild != null)
                        {
                            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, guild.IsLeader(Player.Name) || guild.IsViceLeader(Player.Name) ? MessageMode.ChannelHighlight : MessageMode.Channel, channel.Id, Message);

                            foreach (var observer in guild.GetMembers() )
                            {
                                Context.AddPacket(observer, showTextOutgoingPacket);
                            }
                        }
                    }
                    else if (channel.Flags.Is(ChannelFlags.Party) && Context.Server.Features.HasFeatureFlag(FeatureFlag.PartyChannel) )
                    {
                        Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

                        if (party != null)
                        {
                            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, MessageMode.Channel, channel.Id, Message);

                            foreach (var observer in party.GetMembers() )
                            {
                                Context.AddPacket(observer, showTextOutgoingPacket);
                            }
                        }
                    }
                    else
                    {
                        if (channel.Flags.Is(ChannelFlags.Trade) || channel.Flags.Is(ChannelFlags.TradeRookgaard) )
                        {
                            PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(Player);

                            if (playerCooldownBehaviour != null)
                            {
                                if (playerCooldownBehaviour.HasCooldown("Trade") )
                                {
                                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouMayOnlyPlaceOneOfferInTwoMinutes) );

                                    return Promise.Break;
                                }

                                playerCooldownBehaviour.AddCooldown("Trade", TimeSpan.FromMinutes(2) );
                            }
                        }

                        ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, (channel.Flags.Is(ChannelFlags.Help) && (Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster) ) ? MessageMode.ChannelHighlight : MessageMode.Channel, channel.Id, Message);

                        foreach (var observer in channel.GetMembers() )
                        {
                            Context.AddPacket(observer, showTextOutgoingPacket);
                        }
                    }

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}