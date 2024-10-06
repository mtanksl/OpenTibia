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
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, "You may not speak into channels as long as you are on level 1.") );

                return Promise.Break;
            }

            PlayerMuteBehaviour playerChannelMuteBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerMuteBehaviour>(Player);

            if (playerChannelMuteBehaviour != null)
            {
                string message;

                if (playerChannelMuteBehaviour.IsMuted(out message) )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, message) );

                    return Promise.Break;
                }      
            }

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
                        if (channel.Id == 6 || channel.Id == 7)
                        {
                            PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(Player);

                            if (playerCooldownBehaviour != null)
                            {
                                if (playerCooldownBehaviour.HasCooldown("Trade") )
                                {
                                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, "You may only place one offer in two minutes.") );

                                    return Promise.Break;
                                }

                                playerCooldownBehaviour.AddCooldown("Trade", TimeSpan.FromMinutes(2) );
                            }
                        }

                        ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, (channel.Id == 9 && (Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster) ) ? TalkType.ChannelOrange : TalkType.ChannelYellow, channel.Id, Message);

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