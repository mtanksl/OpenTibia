using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
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

        public override void Execute(Context context)
        {
            if ( !context.Server.Scripts.CreatureWalkScripts.Any(script => script.OnCreatureWalk(Creature, Creature.Tile, ToTile, context) ) )
            {
                Position toPosition = ToTile.Position;

                Tile fromTile = Creature.Tile;

                Position fromPosition = fromTile.Position;

                byte fromIndex = fromTile.GetIndex(Creature);

                fromTile.RemoveContent(fromIndex);

                byte toIndex = ToTile.AddContent(Creature);

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer == Creature)
                    {
                        foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                        {
                            if (pair.Value.GetRootContainer() is Tile tile)
                            {
                                if ( !tile.Position.IsNextTo(toPosition) )
                                {
                                    observer.Client.ContainerCollection.CloseContainer(pair.Key);

                                    context.AddPacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                                }
                            }
                        }

                        int deltaZ = toPosition.Z - fromPosition.Z;

                        int deltaY = toPosition.Y - fromPosition.Y;

                        int deltaX = toPosition.X - fromPosition.X;

                        if (deltaZ < -1 || deltaZ > 1 || deltaY < -2 || deltaY > 2 || deltaX < -2 || deltaX > 2)
                        {
                            context.AddPacket(observer.Client.Connection, new SendTilesOutgoingPacket(context.Server.Map, observer.Client, toPosition) );
                        }
                        else
                        {
                            if (fromPosition.Z == 7 && toPosition.Z == 8)
                            {
                                context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
                            }
                            else
                            {
                                context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );
                            }

                            Position position = fromPosition;

                            while (deltaZ < 0)
                            {
                                context.AddPacket(observer.Client.Connection, new SendMapUpOutgoingPacket(context.Server.Map, observer.Client, position) );

                                position = position.Offset(0, 0, -1);

                                context.AddPacket(observer.Client.Connection, new SendMapWestOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, 1, 0) ) );

                                context.AddPacket(observer.Client.Connection, new SendMapNorthOutgoingPacket(context.Server.Map, observer.Client, position) );

                                deltaZ++;
                            }

                            while (deltaZ > 0)
                            {
                                context.AddPacket(observer.Client.Connection, new SendMapDownOutgoingPacket(context.Server.Map, observer.Client, position) );

                                position = position.Offset(0, 0, 1);

                                context.AddPacket(observer.Client.Connection, new SendMapEastOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, -1, 0) ) );
                                
                                context.AddPacket(observer.Client.Connection, new SendMapSouthOutgoingPacket(context.Server.Map, observer.Client, position) );

                                deltaZ--;
                            }

                            while (deltaY < 0)
                            {
                                position = position.Offset(0, -1, 0);

                                context.AddPacket(observer.Client.Connection, new SendMapNorthOutgoingPacket(context.Server.Map, observer.Client, position) );

                                deltaY++;
                            }

                            while (deltaY > 0)
                            {
                                position = position.Offset(0, 1, 0);

                                context.AddPacket(observer.Client.Connection, new SendMapSouthOutgoingPacket(context.Server.Map, observer.Client, position) );
                                
                                deltaY--;
                            }

                            while (deltaX < 0)
                            {
                                position = position.Offset(-1, 0, 0);

                                context.AddPacket(observer.Client.Connection, new SendMapWestOutgoingPacket(context.Server.Map, observer.Client, position) );

                                deltaX++;
                            }

                            while (deltaX > 0)
                            {
                                position = position.Offset(1, 0, 0);

                                context.AddPacket(observer.Client.Connection, new SendMapEastOutgoingPacket(context.Server.Map, observer.Client, position) );

                                deltaX--;
                            }
                        }

                        context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(observer) );
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(fromPosition) && observer.Tile.Position.CanSee(toPosition) )
                        {
                            context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );
                        }
                        else if (observer.Tile.Position.CanSee(fromPosition) )
                        {
                            context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
                        }
                        else if (observer.Tile.Position.CanSee(toPosition) )
                        {
                            uint removeId;

                            if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                            {
                                context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, Creature) );
                            }
                            else
                            {
                                context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, removeId, Creature) );
                            }
                        }
                    }                    
                }

                //Event

                if (context.Server.Events.TileRemoveCreature != null)
                {
                    context.Server.Events.TileRemoveCreature(this, new TileRemoveCreatureEventArgs(fromTile, Creature, fromIndex) );
                }

                if (context.Server.Events.TileAddCreature != null)
                {
                    context.Server.Events.TileAddCreature(this, new TileAddCreatureEventArgs(ToTile, Creature, toIndex) );
                }
            }

            base.OnCompleted(context);
        }
    }
}