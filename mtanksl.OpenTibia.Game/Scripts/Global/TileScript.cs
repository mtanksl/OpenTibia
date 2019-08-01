using OpenTibia.Common.Events;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts.Move
{
    public class TileScript : IScript
    {
        private Dictionary<ushort, ushort> push = new Dictionary<ushort, ushort>() { { 426, 425 } };

        private Dictionary<ushort, ushort> pull = new Dictionary<ushort, ushort>() { { 425, 426 } };

        public void Start(Server server)
        {
            server.Events.TileAddCreature += OnTileAddCreature;

            server.Events.TileRemoveCreature += OnTileRemoveCreature;
        }

        public void Stop(Server server)
        {
            server.Events.TileAddCreature -= OnTileAddCreature;

            server.Events.TileRemoveCreature -= OnTileRemoveCreature;
        }

        public void OnTileAddCreature(object sender, TileAddCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            if (e.Tile.Ground != null && push.TryGetValue(e.Tile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(e.Tile.Ground, toOpenTibiaId).Execute(e.Server, e.Context);
            }
        }

        public void OnTileRemoveCreature(object sender, TileRemoveCreatureEventArgs e)
        {
            ushort toOpenTibiaId;

            if (e.Tile.Ground != null && pull.TryGetValue(e.Tile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(e.Tile.Ground, toOpenTibiaId).Execute(e.Server, e.Context);
            }
        }
    }
}