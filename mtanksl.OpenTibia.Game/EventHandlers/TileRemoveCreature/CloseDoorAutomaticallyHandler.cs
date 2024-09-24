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
        private static Dictionary<ushort, ushort> horizontalDoors = new Dictionary<ushort, ushort>()
        {
            // Gate of expertise

            { 1230, 1229 },
            { 1248, 1247 },
            { 1262, 1261 },
            { 3541, 3540 },
            { 5104, 5103 },
            { 5122, 5121 },
            { 5295, 5294 },
            { 6209, 6208 },
            { 6266, 6265 },
            { 6897, 6896 },
            { 7039, 7038 },
            { 8558, 8557 },
            { 9182, 9181 },
            { 9284, 9283 },
            { 10285, 10284 },
            { 10474, 10473 },
            { 10781, 10780 },

            // Sealed door

            { 1226, 1225 },
            { 1244, 1243 },
            { 1258, 1257 },
            { 3543, 3542 },
            { 5106, 5105 },
            { 5124, 5123 },
            { 5291, 5290 },
            { 5746, 5745 },
            { 6205, 6204 },
            { 6262, 6261 },
            { 6899, 6898 },
            { 7041, 7040 },
            { 8554, 8553 },
            { 9178, 9177 },
            { 9280, 9279 },
            { 10281, 10280 },
            { 10476, 10475 },
            { 10783, 10782 }
        };

        private static Dictionary<ushort, ushort> verticalDoors = new Dictionary<ushort, ushort>()
        {
            // Gate of expertise

            { 1228, 1227 },
            { 1246, 1245 },
            { 1260, 1259 },
            { 3550, 3549 },
            { 5113, 5112 },
            { 5131, 5130 },
            { 5293, 5292 },
            { 6207, 6206 },
            { 6264, 6263 },
            { 6906, 6905 },
            { 7048, 7047 },
            { 8556, 8555 },
            { 9180, 9179 },
            { 9282, 9281 },
            { 10283, 10282 },
            { 10483, 10482 },
            { 10790, 10789 },

            // Sealed door

            { 1224, 1223 },
            { 1242, 1241 },
            { 1256, 1255 },
            { 3552, 3551 },
            { 5115, 5114 },
            { 5133, 5132 },
            { 5289, 5288 },
            { 5749, 5748 },
            { 6203, 6202 },
            { 6260, 6259 },
            { 6908, 6907 },
            { 7050, 7049 },
            { 8552, 8551 },
            { 9176, 9175 },
            { 9278, 9277 },
            { 10279, 10278 },
            { 10485, 10484 },
            { 10792, 10791 }
        };

        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            if (e.FromTile.TopCreature == null)
            {
                if (e.FromTile.Field)
                {
                    foreach (var topItem in e.FromTile.GetItems() )
                    {
                        ushort toOpenTibiaId;

                        if (horizontalDoors.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
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
                        else if (verticalDoors.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
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