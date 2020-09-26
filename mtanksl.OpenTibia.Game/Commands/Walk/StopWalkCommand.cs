﻿using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class StopWalkCommand : Command
    {
        public StopWalkCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override void Execute(Context context)
        {
            //Arrange

            //Act

            if (context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) ) )
            {
                //Notify

                context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
            }

            base.Execute(context);
        }
    }
}