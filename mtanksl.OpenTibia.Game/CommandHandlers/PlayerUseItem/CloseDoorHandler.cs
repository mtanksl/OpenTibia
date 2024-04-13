using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class CloseDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static Dictionary<ushort, ushort> horizontalDoors = new Dictionary<ushort, ushort>()
        {
            // Brick
            { 5100, 5099 },
            { 5102, 5101 },
                  
            // Framework
            { 1214, 1213 },
            { 1222, 1221 },
            { 5139, 5138 },
                  
            // Pyramid
            { 1236, 1235 },
            { 1240, 1239 },
                  
            // White stone
            { 1254, 1253 },
            { 5518, 5517 },
                 
            // Stone
            { 5118, 5117 },
            { 5120, 5119 },
            { 5136, 5135 },

            // Stone
            { 6254, 6253 },
            { 6258, 6257 },

            // Fence
            { 1542, 1541 },

            //Table
            { 1637, 1636 },
            { 1641, 1640 },

            //TODO: More items
        };

        private static Dictionary<ushort, ushort> verticalDoors = new Dictionary<ushort, ushort>()
        {
            // Brick
            { 5109, 5108 },
            { 5111, 5110 },
                  
            // Framework
            { 1211, 1210 },
            { 1220, 1219 },
            { 5142, 5141 },
                  
            // Pyramid
            { 1233, 1232 },
            { 1238, 1237 },
                  
            // White stone
            { 1251, 1250 },
            { 5516, 5515 },
                 
            // Stone
            { 5127, 5126 },
            { 5129, 5128 },
            { 5145, 5144 },

            // Stone
            { 6251, 6250 },
            { 6256, 6255 },

            // Fence
            { 1540, 1539 },

             //Table
            { 1635, 1634 },
            { 1639, 1638 },

            //TODO: More items
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (horizontalDoors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.Parent is HouseTile houseTile && command.Item is DoorItem doorItem && !houseTile.House.CanOpenDoor(command.Player.Name, doorItem) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                    return Promise.Break;
                }

                Tile door = (Tile)command.Item.Parent;

                if (door.TopCreature == null)
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                }
                else
                {
                    Tile south = Context.Server.Map.GetTile(door.Position.Offset(0, 1, 0) );

                    if (south != null && south.TopCreature == null)
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                        {
                            List<Promise> promises = new List<Promise>();

                            foreach (var creature in door.GetCreatures().ToList() )
                            {
                                if (creature is Player player)
                                {
                                    PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(command.Player);

                                    if (playerIdleBehaviour != null)
                                    {
                                        playerIdleBehaviour.SetNextWalk(TimeSpan.FromMilliseconds(command.Player.Tile.Position.ToDiagonalCost( ( (Tile)item.Parent ).Position) * 1000 * ( (Tile)item.Parent ).Ground.Metadata.Speed / command.Player.Speed) );
                                    }
                                }

                                promises.Add(Context.AddCommand(new CreatureMoveCommand(creature, south) ) );
                            }

                            return Promise.WhenAll(promises.ToArray() );                                   
                        } );
                    }
                    else
                    {
                        Tile north = Context.Server.Map.GetTile(door.Position.Offset(0, -1, 0) );

                        if (north != null && north.TopCreature == null)
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                            {
                                List<Promise> promises = new List<Promise>();

                                foreach (var creature in door.GetCreatures().ToList() )
                                {
                                    if (creature is Player player)
                                    {
                                        PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(command.Player);

                                        if (playerIdleBehaviour != null)
                                        {
                                            playerIdleBehaviour.SetNextWalk(TimeSpan.FromMilliseconds(command.Player.Tile.Position.ToDiagonalCost( ( (Tile)item.Parent ).Position) * 1000 * ( (Tile)item.Parent ).Ground.Metadata.Speed / command.Player.Speed) );
                                        }
                                    }

                                    promises.Add(Context.AddCommand(new CreatureMoveCommand(creature, north) ) );
                                }

                                return Promise.WhenAll(promises.ToArray() );                                   
                            } );
                        }
                    }
                }
            }
            else if (verticalDoors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.Parent is HouseTile houseTile && command.Item is DoorItem doorItem && !houseTile.House.CanOpenDoor(command.Player.Name, doorItem) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                    return Promise.Break;
                }

                Tile door = (Tile)command.Item.Parent;

                if (door.TopCreature == null)
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                }
                else
                {
                    Tile east = Context.Server.Map.GetTile(door.Position.Offset(1, 0, 0) );

                    if (east != null && east.TopCreature == null)
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                        {
                            List<Promise> promises = new List<Promise>();

                            foreach (var creature in door.GetCreatures().ToList() )
                            {
                                if (creature is Player player)
                                {
                                    PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(command.Player);

                                    if (playerIdleBehaviour != null)
                                    {
                                        playerIdleBehaviour.SetNextWalk(TimeSpan.FromMilliseconds(command.Player.Tile.Position.ToDiagonalCost( ( (Tile)item.Parent ).Position) * 1000 * ( (Tile)item.Parent ).Ground.Metadata.Speed / command.Player.Speed) );
                                    }
                                }

                                promises.Add(Context.AddCommand(new CreatureMoveCommand(creature, east) ) );
                            }

                            return Promise.WhenAll(promises.ToArray() );
                        } );
                    }
                    else
                    {
                        Tile west = Context.Server.Map.GetTile(door.Position.Offset(-1, 0, 0) );

                        if (west != null && west.TopCreature == null)
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                            {
                                List<Promise> promises = new List<Promise>();

                                foreach (var creature in door.GetCreatures().ToList() )
                                {
                                    if (creature is Player player)
                                    {
                                        PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(command.Player);

                                        if (playerIdleBehaviour != null)
                                        {
                                            playerIdleBehaviour.SetNextWalk(TimeSpan.FromMilliseconds(command.Player.Tile.Position.ToDiagonalCost( ( (Tile)item.Parent ).Position) * 1000 * ( (Tile)item.Parent ).Ground.Metadata.Speed / command.Player.Speed) );
                                        }
                                    }

                                    promises.Add(Context.AddCommand(new CreatureMoveCommand(creature, west) ) );
                                }

                                return Promise.WhenAll(promises.ToArray() );
                            } );
                        }
                    }
                }
            }

            return next();
        }
    }
}