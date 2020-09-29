using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BucketHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> buckets = new HashSet<ushort>() { 1775, 2005 };

        private HashSet<ushort> drawWell = new HashSet<ushort> { 1368, 1369 };

        private HashSet<ushort> shallowWaters = new HashSet<ushort> { 4608, 4609, 4610, 4611, 4612, 4613, 4612, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4664, 4665, 4666 };

        private HashSet<ushort> swamps = new HashSet<ushort>() { 4691, 4692, 4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712 };

        private HashSet<ushort> lavas = new HashSet<ushort>() { 598, 599, 600, 601 };

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (buckets.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            List<Command> commands = new List<Command>();
          
            if (drawWell.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                commands.Add(new ItemUpdateFluidTypeCommand( (FluidItem)command.Item, FluidType.Water) );
            }
            else if (shallowWaters.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                commands.Add(new ItemUpdateFluidTypeCommand( (FluidItem)command.Item, FluidType.Water) );

                commands.Add(new MagicEffectCommand( ( (Tile)command.ToItem.Container).Position, MagicEffectType.BlueRings) );
            }
            else if (swamps.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                commands.Add(new ItemUpdateFluidTypeCommand( (FluidItem)command.Item, FluidType.Slime) );

                commands.Add(new MagicEffectCommand( ( (Tile)command.ToItem.Container).Position, MagicEffectType.GreenRings) );
            }
            else if (lavas.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                commands.Add(new ItemUpdateFluidTypeCommand( (FluidItem)command.Item, FluidType.Lava) );

                commands.Add(new MagicEffectCommand( ( (Tile)command.ToItem.Container).Position, MagicEffectType.FirePlume) );
            }

            return new SequenceCommand(commands.ToArray() );
        }
    }
}