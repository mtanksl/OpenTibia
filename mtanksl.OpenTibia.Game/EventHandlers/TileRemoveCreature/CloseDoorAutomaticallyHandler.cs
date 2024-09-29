using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class CloseDoorAutomaticallyHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        private readonly Dictionary<ushort, ushort> closeHorizontalDoors;
        private readonly Dictionary<ushort, ushort> closeVerticalDoors;

        public CloseDoorAutomaticallyHandler()
        {
            closeHorizontalDoors = new Dictionary<ushort, ushort>();

            foreach (var item in Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.closeHorizontalGateOfExpertiseDoors") )
            {
                closeHorizontalDoors.Add(item.Key, item.Value);
            }

            foreach (var item in Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.closeHorizontalSealedDoors") )
            {
                closeHorizontalDoors.Add(item.Key, item.Value);
            }

            closeVerticalDoors = new Dictionary<ushort, ushort>();

            foreach (var item in Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.closeVerticalGateOfExpertiseDoors") )
            {
                closeVerticalDoors.Add(item.Key, item.Value);
            }

            foreach (var item in Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.closeVerticalSealedDoors") )
            {
                closeVerticalDoors.Add(item.Key, item.Value);
            }
        }
        
        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            if (e.FromTile.TopCreature == null)
            {
                if (e.FromTile.Field)
                {
                    foreach (var topItem in e.FromTile.GetItems() )
                    {
                        ushort toOpenTibiaId;

                        if (closeHorizontalDoors.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                        {
                            return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) ).Then( (item) =>
                            {
                                List<Promise> promises = new List<Promise>();

                                Tile south = Context.Server.Map.GetTile(e.FromTile.Position.Offset(0, 1, 0) );

                                if (south != null)
                                {
                                    foreach (var moveable in e.FromTile.GetItems().Where(i => !i.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) ).Reverse().ToList() )
                                    {
                                        promises.Add(Context.AddCommand(new ItemMoveCommand(moveable, south, 0) ) );
                                    }
                                }

                                return Promise.WhenAll(promises.ToArray() );   
                            } );
                        }
                        else if (closeVerticalDoors.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                        {
                            return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) ).Then( (item) =>
                            {
                                List<Promise> promises = new List<Promise>();

                                Tile east = Context.Server.Map.GetTile(e.FromTile.Position.Offset(1, 0, 0) );

                                if (east != null)
                                {
                                    foreach (var moveable in e.FromTile.GetItems().Where(i => !i.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) ).Reverse().ToList() )
                                    {
                                        promises.Add(Context.AddCommand(new ItemMoveCommand(moveable, east, 0) ) );
                                    }
                                }

                                return Promise.WhenAll(promises.ToArray() );   
                            } );
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}