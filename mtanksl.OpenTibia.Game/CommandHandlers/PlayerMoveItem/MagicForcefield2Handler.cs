using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefield2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> magicForcefields = new HashSet<ushort>() { 1387 };

        public override bool CanHandle(PlayerMoveItemCommand command, Server server)
        {
            if (command.ToContainer is Tile toTile && toTile.TopItem != null && magicForcefields.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerMoveItemCommand command, Server server)
        {
            Tile fromTile = (Tile)command.ToContainer;

            Tile toTile = server.Map.GetTile( ( (TeleportItem)fromTile.TopItem ).Position );

            return new SequenceCommand(

                new CallbackCommand(context =>
                {
                    return context.TransformCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 0, command.Count) );
                } ),

                new MagicEffectCommand(fromTile.Position, MagicEffectType.Teleport),

                new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
        }
    }
}