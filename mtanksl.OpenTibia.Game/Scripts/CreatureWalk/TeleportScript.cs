using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class TeleportScript : ICreatureWalkScript, IItemMoveScript
    {
        private Dictionary<Position, Position> positions = new Dictionary<Position, Position>();

        public void Start(Server server)
        {
            foreach (var tile in server.Map.GetTiles() )
            {
                foreach (var item in tile.GetItems() )
                {
                    if (item is TeleportItem teleport)
                    {
                        positions.Add(tile.Position, teleport.Position);
                    }
                }
            }

            server.Scripts.CreatureWalkScripts.Add(this);

            server.Scripts.ItemMoveScripts.Add(this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnCreatureWalk(Creature creature, Tile fromTile, Tile toTile, Context context)
        {
            Position position;

            if (positions.TryGetValue(toTile.Position, out position) )
            {
                new CreatureMoveCommand(creature, context.Server.Map.GetTile(position) ).Execute(context);

                new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport).Execute(context);

                new MagicEffectCommand(position, MagicEffectType.Teleport).Execute(context);

                return true;
            }

            return false;
        }

        public bool OnItemMove(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Context context)
        {
            if (toContainer is Tile toTile)
            {
                Position position;

                if (positions.TryGetValue(toTile.Position, out position) )
                {
                    new ItemMoveCommand(player, fromItem, context.Server.Map.GetTile(position), 0, count).Execute(context);

                    new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport).Execute(context);

                    new MagicEffectCommand(position, MagicEffectType.Teleport).Execute(context);

                    return true;
                }
            }

            return false;
        }
    }
}