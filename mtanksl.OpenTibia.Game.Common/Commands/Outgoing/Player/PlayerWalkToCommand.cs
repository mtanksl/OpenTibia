using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerWalkToCommand : IncomingCommand
    {
        public PlayerWalkToCommand(Player player, Tile tile)
        {
            Player = player;

            Tile = tile;
        }

        public Player Player { get; set; }

        public Tile Tile { get; set; }

        public override async Promise Execute()
        {
            MoveDirection[] moveDirections = Context.Server.Pathfinding.GetMoveDirections(Player.Tile.Position, Tile.Position, true);

            if (moveDirections.Length == 0)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );

                await Promise.Break;
            }

            PlayerThinkBehaviour playerThinkBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerThinkBehaviour>(Player);

            if (playerThinkBehaviour != null)
            {
                playerThinkBehaviour.StopAttackAndFollow();
            }

            Context.AddPacket(Player, new StopAttackAndFollowOutgoingPacket(0) );
            
            for (int i = 0; i < moveDirections.Length; i++)
            {
                MoveDirection moveDirection = moveDirections[i];

                if (Player.HasSpecialCondition(SpecialCondition.Drunk) )
                {
                    int value = Context.Server.Randomization.Take(0, 20);

                    if (value < 4)
                    {
                        switch (value)
                        {
                            case 0:

                                moveDirection = MoveDirection.North;

                                break;

                            case 1:

                                moveDirection = MoveDirection.East;
                                break;

                            case 2:

                                moveDirection = MoveDirection.South;

                                break;

                            case 3:

                                moveDirection = MoveDirection.West;

                                break;
                        }

                        await Context.AddCommand(new ShowTextCommand(Player, TalkType.MonsterSay, "Hicks!") );
                    }
                }

                Tile fromTile = Player.Tile;

                Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(moveDirection) );

                if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );                    

                    await Promise.Break;
                }

                int diagonalCost = (moveDirection == MoveDirection.NorthWest || moveDirection == MoveDirection.NorthEast || moveDirection == MoveDirection.SouthWest || moveDirection == MoveDirection.SouthEast)? 2 : 1;

                await Context.Server.GameObjectComponents.AddComponent(Player, new PlayerWalkDelayBehaviour(TimeSpan.FromMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.GroundSpeed / Player.ClientSpeed) ) ).Promise;
                
                if (toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );
                    
                    await Promise.Break;
                }

                await Context.AddCommand(new CreatureMoveCommand(Player, toTile) );

                if (i == moveDirections.Length - 1)
                {
                    await Context.Server.GameObjectComponents.AddComponent(Player, new PlayerWalkDelayBehaviour(TimeSpan.FromMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.GroundSpeed / Player.ClientSpeed) ) ).Promise;
                }
            }
        }
    }
}