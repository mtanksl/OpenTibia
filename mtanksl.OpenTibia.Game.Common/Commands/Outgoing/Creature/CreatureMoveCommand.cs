using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureMoveCommand : Command
    {
        public CreatureMoveCommand(Creature creature, Tile toTile)
        {
            Creature = creature;

            ToTile = toTile;
        }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        public override Promise Execute()
        {
            PlayerWalkDelayBehaviour playerWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkDelayBehaviour>(Creature);

            if (playerWalkDelayBehaviour != null)
            {
                Context.Server.GameObjectComponents.RemoveComponent(Creature, playerWalkDelayBehaviour);
            }

            Tile fromTile = Creature.Tile;

            Dictionary<Creature, byte> observerCanSeeFrom = new Dictionary<Creature, byte>();

            HashSet<Creature> fromCanSeeObserver = new HashSet<Creature>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(fromTile.Position) )
            {
                {
                    if (observer is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            observerCanSeeFrom.Add(player, clientIndex);
                        }
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(fromTile.Position) )
                        {
                            observerCanSeeFrom.Add(observer, 0);
                        }
                    }
                }

                {
                    if (Creature is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(observer, out clientIndex) )
                        {
                            fromCanSeeObserver.Add(observer);
                        }
                    }
                    else
                    {
                        if (fromTile.Position.CanSee(observer.Tile.Position) )
                        {
                            fromCanSeeObserver.Add(observer);
                        }
                    }
                }
            }

            int fromIndex = fromTile.GetIndex(Creature);

            fromTile.RemoveContent(fromIndex);

            Context.Server.Map.ZoneRemoveCreature(fromTile.Position, Creature);

            int toIndex = ToTile.AddContent(Creature);

            Context.Server.Map.ZoneAddCreature(ToTile.Position, Creature);

            Dictionary<Creature, byte> observerCanSeeTo = new Dictionary<Creature, byte>();

            HashSet<Creature> toCanSeeObserver = new HashSet<Creature>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(ToTile.Position) )
            {
                {
                    if (observer is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            observerCanSeeTo.Add(player, clientIndex);
                        }
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(ToTile.Position) )
                        {
                            observerCanSeeTo.Add(observer, 0);
                        }
                    }
                }

                {
                    if (Creature is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(observer, out clientIndex) )
                        {
                            toCanSeeObserver.Add(observer);
                        }
                    }
                    else
                    {
                        if (ToTile.Position.CanSee(observer.Tile.Position) )
                        {
                            toCanSeeObserver.Add(observer);
                        }
                    }
                }
            }

            {
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
                        Context.AddPacket(player, new SendTilesOutgoingPacket(Context.Server.Map, player.Client, ToTile.Position) );
                    }
                    else
                    {
                        if (observerCanSeeFrom.ContainsKey(player) && observerCanSeeTo.ContainsKey(player) )
                        {
                            Context.AddPacket(player, new WalkOutgoingPacket(fromTile.Position, observerCanSeeFrom[player], ToTile.Position) );

                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                Context.AddPacket(player, new SendTileOutgoingPacket(Context.Server.Map, player.Client, fromTile.Position) );
                            }
                        }
                        else if (observerCanSeeFrom.ContainsKey(player) )
                        {
                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                Context.AddPacket(player, new SendTileOutgoingPacket(Context.Server.Map, player.Client, fromTile.Position) );
                            }
                            else
                            {
                                Context.AddPacket(player, new ThingRemoveOutgoingPacket(fromTile.Position, observerCanSeeFrom[player] ) );
                            }
                        }
                        else if (observerCanSeeTo.ContainsKey(player) )
                        {
                            uint removeId;

                            if (player.Client.Battles.IsKnownCreature(Creature.Id, out removeId) )
                            {
                                Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, observerCanSeeTo[player], Creature, player.Client.GetSkullIcon(Creature), player.Client.GetPartyIcon(Creature) ) );
                            }
                            else
                            {
                                Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, observerCanSeeTo[player], removeId, Creature, player.Client.GetSkullIcon(Creature), player.Client.GetPartyIcon(Creature), player.Client.GetWarIcon(Creature) ) );
                            }
                        }

                        Position position = fromTile.Position;

                        while (deltaZ < 0)
                        {
                            Context.AddPacket(player, new SendMapUpOutgoingPacket(Context.Server.Map, player.Client, position) );

                            Context.AddPacket(player, new SendMapWestOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 1, -1) ) );

                            Context.AddPacket(player, new SendMapNorthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 0, -1) ) );

                            position = position.Offset(0, 0, -1);

                            deltaZ++;
                        }

                        while (deltaZ > 0)
                        {
                            Context.AddPacket(player, new SendMapDownOutgoingPacket(Context.Server.Map, player.Client, position) );

                            Context.AddPacket(player, new SendMapEastOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, -1, 1) ) );

                            Context.AddPacket(player, new SendMapSouthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 0, 1) ) );

                            position = position.Offset(0, 0, 1);

                            deltaZ--;
                        }

                        while (deltaY < 0)
                        {
                            Context.AddPacket(player, new SendMapNorthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, -1, 0) ) );

                            position = position.Offset(0, -1, 0);

                            deltaY++;
                        }

                        while (deltaY > 0)
                        {
                            Context.AddPacket(player, new SendMapSouthOutgoingPacket(Context.Server.Map, player.Client, position.Offset(0, 1, 0) ) );

                            position = position.Offset(0, 1, 0);

                            deltaY--;
                        }

                        while (deltaX < 0)
                        {
                            Context.AddPacket(player, new SendMapWestOutgoingPacket(Context.Server.Map, player.Client, position.Offset(-1, 0, 0) ) );

                            position = position.Offset(-1, 0, 0);

                            deltaX++;
                        }

                        while (deltaX > 0)
                        {
                            Context.AddPacket(player, new SendMapEastOutgoingPacket(Context.Server.Map, player.Client, position.Offset(1, 0, 0) ) );

                            position = position.Offset(1, 0, 0);

                            deltaX--;
                        }
                    }
                }
            }

            foreach (var observer in observerCanSeeFrom.Keys.Intersect(observerCanSeeTo.Keys) )
            {
                if (observer is Player player && player != Creature)
                {
                    Context.AddPacket(player, new WalkOutgoingPacket(fromTile.Position, observerCanSeeFrom[player], ToTile.Position) );

                    if (fromTile.Count >= Constants.ObjectsPerPoint)
                    {
                        Context.AddPacket(player, new SendTileOutgoingPacket(Context.Server.Map, player.Client, fromTile.Position) );
                    }
                }
            }

            foreach (var observer in observerCanSeeFrom.Keys.Except(observerCanSeeTo.Keys) )
            {
                if (observer is Player player && player != Creature)
                {
                    if (fromTile.Count >= Constants.ObjectsPerPoint)
                    {
                        Context.AddPacket(player, new SendTileOutgoingPacket(Context.Server.Map, player.Client, fromTile.Position) );
                    }
                    else
                    {
                        Context.AddPacket(player, new ThingRemoveOutgoingPacket(fromTile.Position, observerCanSeeFrom[player] ) );
                    }
                }

                Context.AddEvent(observer, new CreatureDisappearEventArgs(Creature, fromTile) );
            }

            foreach (var observer in fromCanSeeObserver.Except(toCanSeeObserver) )
            {
                if (observer != Creature)
                {
                    Context.AddEvent(Creature, new CreatureDisappearEventArgs(observer, observer.Tile) );
                }
            }

            Context.AddEvent(new TileRemoveCreatureEventArgs(Creature, fromTile, fromIndex, ToTile, toIndex) );

            foreach (var observer in observerCanSeeTo.Keys.Except(observerCanSeeFrom.Keys) )
            {
                if (observer is Player player && player != Creature)
                {
                    uint removeId;

                    if (player.Client.Battles.IsKnownCreature(Creature.Id, out removeId) )
                    {
                        Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, observerCanSeeTo[player], Creature, player.Client.GetSkullIcon(Creature), player.Client.GetPartyIcon(Creature) ) );
                    }
                    else
                    {
                        Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, observerCanSeeTo[player], removeId, Creature, player.Client.GetSkullIcon(Creature), player.Client.GetPartyIcon(Creature), player.Client.GetWarIcon(Creature) ) );
                    }
                }

                Context.AddEvent(observer, new CreatureAppearEventArgs(Creature, ToTile) );
            }

            foreach (var observer in toCanSeeObserver.Except(fromCanSeeObserver) )
            {
                if (observer != Creature)
                {
                    Context.AddEvent(Creature, new CreatureAppearEventArgs(observer, observer.Tile) );
                }
            }

            Context.AddEvent(new TileAddCreatureEventArgs(Creature, fromTile, fromIndex, ToTile, toIndex) );

            return Promise.Completed;
        }
    }
}