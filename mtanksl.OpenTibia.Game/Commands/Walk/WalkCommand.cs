using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class WalkCommand : Command
    {
        public WalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }


        private int index = 0;

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            Tile toTile = server.Map.GetTile( fromTile.Position.Offset(MoveDirection) );

            //Act

            if (index == 0)
            {
                if ( CanWalk(toTile, server, context) )
                {
                    index++;

                    server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), 1000 * fromTile.Ground.Metadata.Speed / Player.Speed, this);
                }
            }
            else
            {
                CreatureWalk(fromTile, toTile, server, context, () =>
                {
                    new CreatureMoveCommand(Player, toTile).Execute(server, context);
                } );
            }            
        }

        protected bool CanWalk(Tile toTile, Server server, CommandContext context)
        {
            if ( toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable)) || toTile.GetCreatures().Any(c => c.Block) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible), 
                    
                                                        new StopWalkOutgoingPacket(Player.Direction) );

                return false;
            }

            return true;
        }

        protected void CreatureWalk(Tile fromTile, Tile toTile, Server server, CommandContext context, Action howToProceed)
        {
            if ( !server.CreatureWalkScripts.Any(script => script.Execute(Player, fromTile, toTile, server, context) ) )
            {
                howToProceed();
            }

            base.Execute(server, context);
        }
    }
}