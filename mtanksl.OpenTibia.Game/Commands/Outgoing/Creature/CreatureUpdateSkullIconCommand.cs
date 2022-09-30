using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateSkullIconCommand : Command
    {
        public CreatureUpdateSkullIconCommand(Creature creature, SkullIcon skullIcon)
        {
            Creature = creature;

            SkullIcon = skullIcon;
        }

        public Creature Creature { get; set; }

        public SkullIcon SkullIcon { get; set; }

        public override void Execute(Context context)
        {
            if (Creature.SkullIcon != SkullIcon)
            {
                Creature.SkullIcon = SkullIcon;

                Tile fromTile = Creature.Tile;

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new SetSkullIconOutgoingPacket(Creature.Id, Creature.SkullIcon) );
                    }
                }
            }

            OnComplete(context);
        }
    }
}