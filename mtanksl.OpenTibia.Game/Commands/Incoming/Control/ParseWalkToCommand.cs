using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkToKnownPathCommand : IncomingCommand
    {
        public ParseWalkToKnownPathCommand(Player player, MoveDirection[] moveDirections)
        {
            Player = player;

            MoveDirections = moveDirections;
        }

        public Player Player { get; set; }

        public MoveDirection[] MoveDirections { get; set; }

        public override async Promise Execute()
        {
            PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(Player);

            if (playerIdleBehaviour != null)
            {
                TimeSpan executeIn;

                if ( !playerIdleBehaviour.CanWalk(out executeIn) )
                {
                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                    await Context.Server.GameObjectComponents.AddComponent(Player, new PlayerWalkedDelayBehaviour(executeIn) ).Promise;
                }

                playerIdleBehaviour.SetLastAction();
            }

            PlayerWalkingDelayBehaviour playerWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkingDelayBehaviour>(Player);

            if (playerWalkDelayBehaviour != null)
            {
                if (Context.Server.GameObjectComponents.RemoveComponent(Player, playerWalkDelayBehaviour) )
                {
                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            PlayerWalkedDelayBehaviour playerActionDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkedDelayBehaviour>(Player);

            if (playerActionDelayBehaviour != null)
            {
                Context.Server.GameObjectComponents.RemoveComponent(Player, playerActionDelayBehaviour);
            }

            foreach (var moveDirection in MoveDirections)
            {
                Tile fromTile = Player.Tile;

                Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(moveDirection) );

                if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                    await Promise.Break;
                }

                await Context.Server.GameObjectComponents.AddComponent(Player, new PlayerWalkingDelayBehaviour(TimeSpan.FromMilliseconds(fromTile.Position.ToDiagonalCost(toTile.Position) * 1000 * toTile.Ground.Metadata.Speed / Player.Speed) ) ).Promise;
                
                if (toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                    await Promise.Break;
                }

                await Context.AddCommand(new CreatureMoveCommand(Player, toTile) );
            }
        }
    }
}