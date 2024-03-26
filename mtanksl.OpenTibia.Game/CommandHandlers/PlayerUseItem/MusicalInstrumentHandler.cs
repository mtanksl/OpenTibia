using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MusicalInstrumentHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> green = new HashSet<ushort>() { 2070, 2071, 2073, 2075, 2076, 2078, 2367, 2374 };

        private HashSet<ushort> purple = new HashSet<ushort>() { 2079 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (green.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.GreenNotes) );
            }
            else if (purple.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.PurpleNotes) );
            }

            return next();
        }
    }
}