﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseInvitePlayerChannelCommand : IncomingCommand
    {
        public ParseInvitePlayerChannelCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            // #i <player>

            PrivateChannel privateChannel = Context.Server.Channels.GetPrivateChannel(Player);

            if (privateChannel != null)
            {
                Player observer = Context.Server.GameObjects.GetPlayerByName(Name);

                if (observer != null && observer != Player)
                {
                    if ( !privateChannel.ContainerMember(observer) && !privateChannel.ContainsInvitation(observer) )
                    {
                        privateChannel.AddInvitation(observer);

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, observer.Name + " has been invited.") );

                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(MessageMode.Look, Player.Name + " invites you to " + (Player.Gender == Gender.Male ? "his" : "her") + " private chat channel." ) );

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}