using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkToCommand : IncomingCommand
    {
        public ParseWalkToCommand(Player player, MoveDirection[] moveDirections)
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
                playerIdleBehaviour.SetLastAction();
            }

            PlayerWalkDelayBehaviour playerWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkDelayBehaviour>(Player);

            if (playerWalkDelayBehaviour != null)
            {
                if (Context.Server.GameObjectComponents.RemoveComponent(Player, playerWalkDelayBehaviour) )
                {
                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            for (int i = 0; i < MoveDirections.Length; i++)
            {
                MoveDirection moveDirection = MoveDirections[i];

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

                        await Context.AddCommand(new ShowTextCommand(Player, MessageMode.MonsterSay, "Hicks!") );
                    }
                }

                Tile fromTile = Player.Tile;

                Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(moveDirection) );

                if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                    await Promise.Break;
                }

                int diagonalCost = (moveDirection == MoveDirection.NorthWest || moveDirection == MoveDirection.NorthEast || moveDirection == MoveDirection.SouthWest || moveDirection == MoveDirection.SouthEast) ? 2 : 1;

                await Context.Server.GameObjectComponents.AddComponent(Player, new PlayerWalkDelayBehaviour(TimeSpan.FromMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.GroundSpeed / Player.ClientSpeed) ) ).Promise;
                
                if (toTile.NotWalkable || toTile.Block)
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.SorryNotPossible) );

                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

                    await Promise.Break;
                }

                await Context.AddCommand(new CreatureMoveCommand(Player, toTile) );
            }
        }
    }
}