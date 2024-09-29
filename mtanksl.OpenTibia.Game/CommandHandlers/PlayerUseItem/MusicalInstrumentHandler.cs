using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MusicalInstrumentHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> greenMusicalInstruments;
        private readonly HashSet<ushort> purpleMusicalInstruments;

        public MusicalInstrumentHandler()
        {
            greenMusicalInstruments = Context.Server.Values.GetUInt16HashSet("values.items.greenMusicalInstruments");
            purpleMusicalInstruments = Context.Server.Values.GetUInt16HashSet("values.items.purpleMusicalInstruments");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (greenMusicalInstruments.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.GreenNotes) );
            }
            else if (purpleMusicalInstruments.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.PurpleNotes) );
            }

            return next();
        }
    }
}