using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CloseDoorAutomaticallyHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        private static Dictionary<ushort, ushort> doors = new Dictionary<ushort, ushort>()
        {
            // Gate of expertise

            { 1228, 1227 },
            { 1230, 1229 },

            { 1246, 1245 },
            { 1248, 1247 },

            { 1260, 1259 },
            { 1262, 1261 },

            { 3541, 3540 },
            { 3550, 3549 },

            { 5104, 5103 },
            { 5113, 5112 },

            { 5122, 5121 },
            { 5131, 5130 },

            { 5293, 5292 },
            { 5295, 5294 },

            { 6207, 6206 },
            { 6209, 6208 },

            { 6264, 6263 },
            { 6266, 6265 },

            { 6897, 6896 },
            { 6906, 6905 },

            { 7039, 7038 },
            { 7048, 7047 },

            { 8556, 8555 },
            { 8558, 8557 },

            { 9180, 9179 },
            { 9182, 9181 },

            { 9282, 9281 },
            { 9284, 9283 },

            { 10283, 10282 },
            { 10285, 10284 },

            { 10474, 10473 },
            { 10483, 10482 },

            { 10781, 10780 },
            { 10790, 10789 },

            // Sealed door

            { 1224, 1223 },
            { 1226, 1225 },

            { 1242, 1241 },
            { 1244, 1243 },

            { 1256, 1255 },
            { 1258, 1257 },

            { 3543, 3542 },
            { 3552, 3551 },

            { 5106, 5105 },
            { 5115, 5114 },

            { 5124, 5123 },
            { 5133, 5132 },

            { 5289, 5288 },
            { 5291, 5290 },

            { 5746, 5745 },
            { 5749, 5748 },

            { 6203, 6202 },
            { 6205, 6204 },

            { 6260, 6259 },
            { 6262, 6261 },

            { 6899, 6898 },
            { 6908, 6907 },

            { 7041, 7040 },
            { 7050, 7049 },

            { 8552, 8551 },
            { 8554, 8553 },

            { 9176, 9175 },
            { 9178, 9177 },

            { 9278, 9277 },
            { 9280, 9279 },

            { 10279, 10278 },
            { 10281, 10280 },

            { 10476, 10475 },
            { 10485, 10484 },

            { 10783, 10782 },
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

                        if (doors.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                        {
                            return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}