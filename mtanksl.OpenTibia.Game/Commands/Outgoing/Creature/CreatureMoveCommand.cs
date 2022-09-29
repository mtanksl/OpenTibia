using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

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
            Tile fromTile = Creature.Tile;

            byte fromIndex = fromTile.GetIndex(Creature);

            fromTile.RemoveContent(fromIndex);

            byte toIndex = ToTile.AddContent(Creature);

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (Creature == observer)
                {
                    int deltaZ = ToTile.Position.Z - fromTile.Position.Z;

                    int deltaY = ToTile.Position.Y - fromTile.Position.Y;

                    int deltaX = ToTile.Position.X - fromTile.Position.X;

                    if (deltaZ < -1 || deltaZ > 1 || deltaY < -2 || deltaY > 2 || deltaX < -2 || deltaX > 2)
                    {
                        context.AddPacket(observer.Client.Connection, new SendTilesOutgoingPacket(context.Server.Map, observer.Client, ToTile.Position) );
                    }
                    else
                    {
                        if (fromTile.Position.Z == 7 && ToTile.Position.Z == 8)
                        {
                            context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex) );
                        }
                        else
                        {
                            context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromTile.Position, fromIndex, ToTile.Position) );
                        }

                        Position position = fromTile.Position;

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
                }
                else
                {
                    bool canSeeFrom = observer.Tile.Position.CanSee(fromTile.Position);

                    bool canSeeTo = observer.Tile.Position.CanSee(ToTile.Position);

                    if (canSeeFrom && canSeeTo)
                    {
                        context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromTile.Position, fromIndex, ToTile.Position) );
                    }
                    else if (canSeeFrom)
                    {
                        context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex) );
                    }
                    else if (canSeeTo)
                    {
                        uint removeId;

                        if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                        {
                            context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, toIndex, Creature) );
                        }
                        else
                        {
                            context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, toIndex, removeId, Creature) );
                        }
                    }
                }
            }

            base.Execute(context);
        }
    }
}