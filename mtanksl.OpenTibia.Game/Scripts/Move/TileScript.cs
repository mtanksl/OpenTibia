using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts.Move
{
    public class TileScript : ITileAddCreatureScript, ITileRemoveCreatureScript
    {
        public void Register(Server server)
        {
            server.Scripts.TileAddCreatureScripts.Add(this);

            server.Scripts.TileRemoveCreatureScripts.Add(this);
        }

        private Dictionary<ushort, ushort> stepOut = new Dictionary<ushort, ushort>()
        {
            { 425, 426 }
        };

        public void OnTileRemoveCreature(Creature creature, Tile fromTile, byte fromIndex, Server server, CommandContext context)
        {
            ushort toOpenTibiaId;

            if (fromTile.Ground != null && stepOut.TryGetValue(fromTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(fromTile.Ground, toOpenTibiaId).Execute(server, context);
            }
        }

        private Dictionary<ushort, ushort> stepIn = new Dictionary<ushort, ushort>()
        {
            { 426, 425 }
        };

        public void OnTileAddCreature(Creature creature, Tile fromTile, byte fromIndex, Server server, CommandContext context)
        {
            ushort toOpenTibiaId;

            if (fromTile.Ground != null && stepIn.TryGetValue(fromTile.Ground.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                new ItemTransformCommand(fromTile.Ground, toOpenTibiaId).Execute(server, context);
            }
        }
    }
}