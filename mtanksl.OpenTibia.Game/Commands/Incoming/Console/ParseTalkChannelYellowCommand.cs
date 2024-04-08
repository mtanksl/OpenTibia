﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

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
            Channel channel = Context.Server.Channels.GetChannel(ChannelId);

            if (channel != null)
            {
                if (channel.ContainerMember(Player) )
                {
                    if (channel.Id == 0)
                    {
                        Guild guild = Context.Server.Guilds.GetGuildThatContainsMember(Player);

                        if (guild != null)
                        {
                            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.ChannelYellow, channel.Id, Message);

                            foreach (var observer in guild.GetMembers() )
                            {
                                Context.AddPacket(observer, showTextOutgoingPacket);
                            }
                        }
                    }
                    else if (channel.Id == 1)
                    {
                        Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

                        if (party != null)
                        {
                            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.ChannelYellow, channel.Id, Message);

                            foreach (var observer in party.GetMembers() )
                            {
                                Context.AddPacket(observer, showTextOutgoingPacket);
                            }
                        }
                    }
                    else
                    {
                        if (channel.Id == 9 && (Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster) )
                        {
                            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.ChannelOrange, channel.Id, Message);

                            foreach (var observer in channel.GetMembers() )
                            {
                                Context.AddPacket(observer, showTextOutgoingPacket);
                            }
                        }
                        else
                        {
                            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.ChannelYellow, channel.Id, Message);

                            foreach (var observer in channel.GetMembers() )
                            {
                                Context.AddPacket(observer, showTextOutgoingPacket);
                            }
                        }
                    }

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}