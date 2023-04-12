using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CloseDoorAutomaticallyHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        private Dictionary<ushort, ushort> doors = new Dictionary<ushort, ushort>()
        {
            // Brick
            { 5104, 5103 },
            { 5113, 5112 },

            { 5106, 5105 },
            { 5115, 5114 },

            // Framework
            { 1228, 1227 },
            { 1230, 1229 },

            { 1224, 1223 },
            { 1226, 1225 },

            // Pyramid
            { 1246, 1245 },
            { 1248, 1247 },

            { 1242, 1241 },
            { 1244, 1243 },

            // White stone
            { 1260, 1259 },
            { 1262, 1261 },

            { 1256, 1255 },
            { 1258, 1257 },

            // Stone
            { 5122, 5121 },
            { 5131, 5130 },

            { 5124, 5123 },
            { 5133, 5132 },
            
            //TODO: More items
        };

        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            foreach (var topItem in e.Tile.GetItems() )
            {
                ushort toOpenTibiaId;

                if (doors.TryGetValue(topItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new ItemTransformCommand(topItem, toOpenTibiaId, 1) );
                }
            }

            return Promise.Completed;
        }
    }
}