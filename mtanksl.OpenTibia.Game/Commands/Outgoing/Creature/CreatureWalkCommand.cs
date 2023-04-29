using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureWalkCommand : Command
    {
        public CreatureWalkCommand(Creature creature, Tile toTile) : this(creature, toTile, null)
        {

        }

        public CreatureWalkCommand(Creature creature, Tile toTile, Direction? changeDirectionOnMove)
        {
            Creature = creature;

            ToTile = toTile;

            ChangeDirectionOnMove = changeDirectionOnMove;
        }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        public Direction? ChangeDirectionOnMove { get; set; }

        public override Promise Execute()
        {
            var canSeeFrom = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.Map.GetObservers(Creature.Tile.Position).OfType<Player>() )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                {
                    canSeeFrom.Add(observer, clientIndex);
                }
            }

            Tile fromTile = Creature.Tile;

            byte fromIndex = fromTile.GetIndex(Creature);

            fromTile.RemoveContent(fromIndex);

            Context.Server.Map.RemoveObserver(fromTile.Position, Creature);

            byte toIndex = ToTile.AddContent(Creature);

            Context.Server.Map.AddObserver(ToTile.Position, Creature);

            var canSeeTo = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.Map.GetObservers(Creature.Tile.Position).OfType<Player>() )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                {
                    canSeeTo.Add(observer, clientIndex);
                }
            }

            Direction? direction = fromTile.Position.ToDirection(ToTile.Position);

            if (direction != null)
            {
                Creature.Direction = direction.Value;
            }

            if (Creature is Player player)
            {
                int deltaZ = ToTile.Position.Z - fromTile.Position.Z;

                int deltaY = ToTile.Position.Y - fromTile.Position.Y;

                int deltaX = ToTile.Position.X - fromTile.Position.X;

                if (deltaZ < -1 || deltaZ > 1 || deltaY < -6 || deltaY > 7 || deltaX < -8 || deltaX > 9 || (fromTile.Position.Z == 7 && ToTile.Position.Z == 8) )
                {
                    Context.AddPacket(player.Client.Connection, new SendTilesOutgoingPacket(Context.Server.Map, player.Client, ToTile.Position) );
                }
                else
                {
                    if (canSeeFrom.ContainsKey(player) && canSeeTo.ContainsKey(player) )
                    {
                        Context.AddPacket(player.Client.Connection, new WalkOutgoingPacket(fromTile.Position, canSeeFrom[player], ToTile.Position) );

                        if (fromTile.Count >= Constants.ObjectsPerPoint)
                        {
                            Context.AddPacket(player.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, player.Client, fromTile.Position) );
                        }
                    }
                    else if (canSeeFrom.ContainsKey(player) )
                    {
                        if (fromTile.Count >= Constants.ObjectsPerPoint)
                        {
                            Context.AddPacket(player.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, player.Client, fromTile.Position) );
                        }
                        else
                        {
                            Context.AddPacket(player.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, canSeeFrom[player] ) );
                        }
                    }
                    else if (canSeeTo.ContainsKey(player) )
                    {
                        uint removeId;

                        if (player.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                        {
                            Context.AddPacket(player.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, canSeeTo[player], Creature) );
                        }
                        else
                        {
                            Context.AddPacket(player.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, canSeeTo[player], removeId, Creature) );
                        }
                    }

                    Position position = fromTile.Position;

                    while (deltaZ < 0)
                    {
                        Context.AddPacket(player.Client.Connection, new SendMapUpOutgoingPacket(Context.Server.Map, player.Client, position),

                                                                    new SendMapWestOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 1, -1) ),

                                                                    new SendMapNorthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 0, -1) ) );

                        position = position.Offset(0, 0, -1);

                        deltaZ++;
                    }

                    while (deltaZ > 0)
                    {
                        Context.AddPacket(player.Client.Connection, new SendMapDownOutgoingPacket(Context.Server.Map, player.Client, position),

                                                                    new SendMapEastOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, -1, 1) ),

                                                                    new SendMapSouthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 0, 1) ) );

                        position = position.Offset(0, 0, 1);

                        deltaZ--;
                    }

                    while (deltaY < 0)
                    {
                        Context.AddPacket(player.Client.Connection, new SendMapNorthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, -1, 0) ) );

                        position = position.Offset(0, -1, 0);

                        deltaY++;
                    }

                    while (deltaY > 0)
                    {
                        Context.AddPacket(player.Client.Connection, new SendMapSouthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 1, 0) ) );

                        position = position.Offset(0, 1, 0);

                        deltaY--;
                    }

                    while (deltaX < 0)
                    {
                        Context.AddPacket(player.Client.Connection, new SendMapWestOutgoingPacket(Context.Server.Map, player.Client, position.Offset(-1, 0, 0) ) );

                        position = position.Offset(-1, 0, 0);

                        deltaX++;
                    }

                    while (deltaX > 0)
                    {
                        Context.AddPacket(player.Client.Connection, new SendMapEastOutgoingPacket(Context.Server.Map, player.Client, position.Offset(1, 0, 0) ) );

                        position = position.Offset(1, 0, 0);

                        deltaX--;
                    }
                }     
                
                PlayerActionDelayBehaviour playerActionDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerActionDelayBehaviour>(player);

                if (playerActionDelayBehaviour != null)
                {
                    Context.Server.GameObjectComponents.RemoveComponent(player, playerActionDelayBehaviour);
                }

                PlayerWalkDelayBehaviour playerWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkDelayBehaviour>(player);

                if (playerWalkDelayBehaviour != null)
                {
                    if (Context.Server.GameObjectComponents.RemoveComponent(player, playerWalkDelayBehaviour) )
                    {
                        Context.AddPacket(player.Client.Connection, new StopWalkOutgoingPacket(player.Direction) );
                    }
                }

                Context.AddEvent(player, new CreatureWalkEventArgs(Creature, fromTile, fromIndex, ToTile, toIndex) );
            }

            foreach (var observer in canSeeFrom.Keys.Intersect(canSeeTo.Keys) )
            {
                if (observer != Creature)
                {
                    Context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromTile.Position, canSeeFrom[observer], ToTile.Position) );

                    if (fromTile.Count >= Constants.ObjectsPerPoint)
                    {
                        Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, fromTile.Position) );
                    }

                    Context.AddEvent(observer, new CreatureWalkEventArgs(Creature, fromTile, fromIndex, ToTile, toIndex) );
                }
            }

            foreach (var observer in canSeeFrom.Keys.Except(canSeeTo.Keys) )
            {
                if (observer != Creature)
                {
                    if (fromTile.Count >= Constants.ObjectsPerPoint)
                    {
                        Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, fromTile.Position) );
                    }
                    else
                    {
                        Context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, canSeeFrom[observer] ) );
                    }

                    Context.AddEvent(observer, new CreatureWalkEventArgs(Creature, fromTile, fromIndex, ToTile, toIndex) );
                }
            }

            foreach (var observer in canSeeTo.Keys.Except(canSeeFrom.Keys) )
            {
                if (observer != Creature)
                {
                    uint removeId;

                    if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                    {
                        Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, canSeeTo[observer], Creature) );
                    }
                    else
                    {
                        Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, canSeeTo[observer], removeId, Creature) );
                    }

                    Context.AddEvent(observer, new CreatureWalkEventArgs(Creature, fromTile, fromIndex, ToTile, toIndex) );
                }
            }

            Context.AddEvent(new TileRemoveCreatureEventArgs(fromTile, Creature, fromIndex) );

            Context.AddEvent(new TileAddCreatureEventArgs(ToTile, Creature, toIndex) );

            Context.AddEvent(new CreatureWalkEventArgs(Creature, fromTile, fromIndex, ToTile, toIndex) );

            return Promise.Completed;
        }
    }
}