using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowMagicEffectCommand : Command
    {
        public ShowMagicEffectCommand(IContent content, MagicEffectType magicEffectType)
        {
            Position position = null;

            switch (content)
            {
                case Item item:

                    switch (item.Root() )
                    {
                        case Tile tile:

                            position = tile.Position;

                            break;

                        case Inventory inventory:

                            position = inventory.Player.Tile.Position;

                            break;

                        case Safe safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    break;

                case Creature creature:

                    position = creature.Tile.Position;

                    break;
            }

            Position = position;

            MagicEffectType = magicEffectType;
        }

        public ShowMagicEffectCommand(Position position, MagicEffectType magicEffectType)
        {
            Position = position;

            MagicEffectType = magicEffectType;
        }

        public Position Position { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public override Promise Execute()
        {
            if (Position != null)
            {
                ShowMagicEffectOutgoingPacket showMagicEffectOutgoingPacket = new ShowMagicEffectOutgoingPacket(Position, MagicEffectType);

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Position) )
                {
                    if (observer.Tile.Position.CanSee(Position) )
                    {
                        Context.AddPacket(observer, showMagicEffectOutgoingPacket);
                    }
                }
            }

            return Promise.Completed;
        }
    }
}