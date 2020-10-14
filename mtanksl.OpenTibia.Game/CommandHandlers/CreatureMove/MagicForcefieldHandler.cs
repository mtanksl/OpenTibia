using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefieldHandler : CommandHandler<CreatureMoveCommand>
    {
        private HashSet<ushort> magicForcefields = new HashSet<ushort>() { 1387 };

        public override bool CanHandle(CreatureMoveCommand command, Server server)
        {
            if (command.ToTile.TopItem != null && magicForcefields.Contains(command.ToTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(CreatureMoveCommand command, Server server)
        {
            Tile fromTile = command.ToTile;

            Tile toTile = server.Map.GetTile( ( (TeleportItem)fromTile.TopItem ).Position );

            return new SequenceCommand(

                new CallbackCommand(context =>
                {
                    return context.TransformCommand(new CreatureMoveCommand(command.Creature, toTile) );
                } ),

                new MagicEffectCommand(fromTile.Position, MagicEffectType.Teleport),

                new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
        }
    }
}