using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateParentCommand : Command
    {
        public CreatureUpdateParentCommand(Creature creature, Tile toTile)
        {
            Creature = creature;

            ToTile = toTile;
        }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Tile fromTile = Creature.Tile;

                byte fromIndex = fromTile.GetIndex(Creature);

                fromTile.RemoveContent(fromIndex);

                byte toIndex = ToTile.AddContent(Creature);

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer == Creature)
                    {
                        context.AddPacket(observer.Client.Connection, new SendTilesOutgoingPacket(context.Server.Map, observer.Client, ToTile.Position) );
                    }
                    else
                    {
                        bool canSeeFrom = observer.Tile.Position.CanSee(fromTile.Position) && fromIndex < Constants.ObjectsPerPoint;

		                bool canSeeTo = observer.Tile.Position.CanSee(ToTile.Position) && toIndex < Constants.ObjectsPerPoint;

                        if (canSeeFrom && canSeeTo)
		                {
                            context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromTile.Position, fromIndex, ToTile.Position) );

                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(context.Server.Map, observer.Client, fromTile.Position) );
                            }
                        }
                        else if (canSeeFrom)
                        {
                            context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex) );

                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(context.Server.Map, observer.Client, fromTile.Position) );
                            }
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

                context.AddEvent(new TileRemoveCreatureEventArgs(fromTile, Creature, fromIndex) );

                context.AddEvent(new TileAddCreatureEventArgs(ToTile, Creature, toIndex) );

                resolve(context);
            } );
        }    
    }
}