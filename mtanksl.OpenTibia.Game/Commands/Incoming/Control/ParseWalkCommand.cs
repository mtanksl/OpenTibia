using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkCommand : IncomingCommand
    {
        public ParseWalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }

        public override Promise Execute()
        {
            PlayerActionDelayBehaviour playerActionDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerActionDelayBehaviour>(Player);

            if (playerActionDelayBehaviour != null)
            {
                Context.Server.GameObjectComponents.RemoveComponent(Player, playerActionDelayBehaviour);
            }

            PlayerWalkDelayBehaviour playerWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkDelayBehaviour>(Player);

            if (playerWalkDelayBehaviour != null)
            {
                if (Context.Server.GameObjectComponents.RemoveComponent(Player, playerWalkDelayBehaviour) )
                {
                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(Player);

            if (playerIdleBehaviour != null)
            {
                playerIdleBehaviour.SetLastActionResponse();
            }

            Tile fromTile = Player.Tile;

            Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(MoveDirection) );

            if (toTile == null)
            {
                Tile toTileDown = Context.Server.Map.GetTile(fromTile.Position.Offset(MoveDirection).Offset(0, 0, 1) );

                if (toTileDown != null)
                {
                    if (toTileDown.Height >= 3)
                    {
                        toTile = toTileDown;
                    }
                }
            }
            else
            {
                Tile fromTileUp = Context.Server.Map.GetTile(fromTile.Position.Offset(0, 0, -1) );

                if (fromTileUp == null)
                {
                    Tile toTileUp = Context.Server.Map.GetTile(toTile.Position.Offset(0, 0, -1) );

                    if (toTileUp != null)
                    {
                        if (fromTile.Height >= 3)
                        {
                            toTile = toTileUp;
                        }
                    }
                }
            }

            if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.Block)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                return Promise.Break;
            }

            return Context.Server.GameObjectComponents.AddComponent(Player, new PlayerWalkDelayBehaviour(TimeSpan.FromMilliseconds(fromTile.Position.ToDiagonalCost(toTile.Position) * 1000 * toTile.Ground.Metadata.Speed / Player.Speed) ) ).Promise.Then( () =>
            {            
                if (toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                    return Promise.Break;
                }

                return Context.AddCommand(new CreatureMoveCommand(Player, toTile) );
            } );
        }
    }
}