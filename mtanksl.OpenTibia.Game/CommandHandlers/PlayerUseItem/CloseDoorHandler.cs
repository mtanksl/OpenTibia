using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CloseDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> horizontalDoors = new Dictionary<ushort, ushort>()
        {
            // Brick
            { 5100, 5099 },
            { 5102, 5101 },
                  
            // Framework
            { 1214, 1213 },
            { 1222, 1221 },
            { 5139, 5138 },
                  
            // Pyramid
            { 1236, 1235 },
            { 1240, 1239 },
                  
            // White stone
            { 1254, 1253 },
            { 5518, 5517 },
                 
            // Stone
            { 5118, 5117 },
            { 5120, 5119 },
            { 5136, 5135 },

            //TODO: More items
        };

        private Dictionary<ushort, ushort> verticalDoors = new Dictionary<ushort, ushort>()
        {
            // Brick
            { 5109, 5108 },
            { 5111, 5110 },
                  
            // Framework
            { 1211, 1210 },
            { 1220, 1219 },
            { 5142, 5141 },
                  
            // Pyramid
            { 1233, 1232 },
            { 1238, 1237 },
                  
            // White stone
            { 1251, 1250 },
            { 5516, 5515 },
                 
            // Stone
            { 5127, 5126 },
            { 5129, 5128 },
            { 5145, 5144 },

            //TODO: More items
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (horizontalDoors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    Tile door = (Tile)item.Parent;

                    Tile south = Context.Server.Map.GetTile(door.Position.Offset(0, 1, 0) );

                    List<Command> commands = new List<Command>();

                    foreach (var creature in door.GetCreatures() )
                    {
                        commands.Add(new CreatureUpdateParentCommand(creature, south) );
                    }

                    return Command.Sequence(commands.ToArray() );
                } );
            }
            else if (verticalDoors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    Tile door = (Tile)item.Parent;

                    Tile east = Context.Server.Map.GetTile(door.Position.Offset(1, 0, 0) );

                    List<Command> commands = new List<Command>();

                    foreach (var creature in door.GetCreatures() )
                    {
                        commands.Add(new CreatureUpdateParentCommand(creature, east) );
                    }

                    return Command.Sequence(commands.ToArray() );
                } );
            }

            return next();
        }
    }
}