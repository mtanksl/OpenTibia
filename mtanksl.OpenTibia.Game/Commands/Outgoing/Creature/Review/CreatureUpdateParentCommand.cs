using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;

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

                byte fromIndex = context.AddCommand(new TileRemoveCreatureCommand(fromTile, Creature) ).Result;

                byte toIndex = context.AddCommand(new TileAddCreatureCommand(ToTile, Creature) ).Result;

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer == Creature)
                    {
                        context.AddPacket(observer.Client.Connection, new SendTilesOutgoingPacket(context.Server.Map, observer.Client, ToTile.Position) );
                    }
                }

                resolve(context);
            } );
        }
    }
}