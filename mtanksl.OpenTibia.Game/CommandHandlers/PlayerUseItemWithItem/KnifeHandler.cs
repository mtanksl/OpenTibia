using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> knifes = new HashSet<ushort>() { 2566 };

        private HashSet<ushort> fruits = new HashSet<ushort>() { 2676, 2677, 2679, 2681, 5097, 2682, 2675, 2673, 8839, 8840, 2674, 2680 };

        private HashSet<ushort> cakes = new HashSet<ushort>() { 6278 };

        private ushort decoratedCake = 6279;

        private HashSet<ushort> pumpkins = new HashSet<ushort>() { 2683 };

        private ushort pumpkinhead = 2096;

        public override bool CanHandle(PlayerUseItemWithItemCommand command, Server server)
        {
            if (knifes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (fruits.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return true;
                }
                else if (pumpkins.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return true;
                }
            }

            return false;
        }

        public override Command Handle(PlayerUseItemWithItemCommand command, Server server)
        {
            List<Command> commands = new List<Command>();

            if (fruits.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                Item extra = command.Player.Inventory.GetContent( (byte)Slot.Extra) as Item;

                if (extra != null && cakes.Contains(extra.Metadata.OpenTibiaId) )
                {
                    if (command.ToItem is StackableItem toStackableItem && toStackableItem.Count > 1)
                    {
                        commands.Add(new ItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count - 1) ) );
                    }
                    else
                    {
                        commands.Add(new ItemDestroyCommand(command.ToItem) );
                    }

                    commands.Add(new ItemTransformCommand(extra, decoratedCake, 1) ); 
                }
            }
            else if (pumpkins.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                commands.Add(new ItemTransformCommand(command.ToItem, pumpkinhead, 1) );
            }

            return new SequenceCommand(commands.ToArray() );
        }
    }
}