using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class BeginWalkCommand : Command
    {
        public BeginWalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }

        public override void Execute(Context context)
        {
            Tile fromTile = Player.Tile;

            if (fromTile != null)
            {
                Tile toTile = context.Server.Map.GetTile(fromTile.Position.Offset(MoveDirection) );

                if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                {
                    context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible), 
                    
                                                                  new StopWalkOutgoingPacket(Player.Direction) );
                }
                else
                {
                    List<Command> commands = new List<Command>();

                    commands.Add(new DelayCommand(Constants.CreatureAttackOrFollowSchedulerEvent(Player), 1000 * fromTile.Ground.Metadata.Speed / Player.Speed) );

                    commands.Add(new EndWalkCommand(Player, MoveDirection) );

                    Command command = new SequenceCommand(commands.ToArray() );

                    command.Completed += (s, e) =>
                    {
                        base.OnCompleted(e.Context);
                    };

                    command.Execute(context);
                }
            }
        }
    }
}