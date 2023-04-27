using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ConstructionKitHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> constructionKits = new Dictionary<ushort, ushort>()
        {
            { 3901, 1652 }, // Wooden chair
            { 3902, 1658 }, // Sofa chair
            { 3903, 1666 }, // Red cushioned chair
            { 3904, 1670 }, // Green cushioned chair
            { 3905, 3813 }, // Tusk chair
            { 3906, 3817 }, // Ivony chair
            { 3907, 3821 }, // Trunk chair
            { 3908, 1619 }, // Small table
            { 3909, 1614 }, // Big table
            { 3910, 1615 }, // Square table
            { 3911, 1616 }, // Round table
            { 3912, 2603 }, // Coal basin
            { 3913, 3805 }, // Stone table
            { 3914, 3807 }, // Tusk table
            { 3915, 1716 }, // Drawer
            { 3916, 1724 }, // Dresser
            { 3917, 1728 }, // Pendulum clock
            { 3918, 1732 }, // Locker
            { 3919, 3809 }, // Bamboo table
            { 3920, 3811 }, // Trunk table
            { 3921, 2084 }, // Harp
            { 3922, 2094 }, // Birdcage
            { 3923, 2098 }, // Globe
            { 3924, 2064 }, // Table lamp
            { 3925, 1674 }, // Rocking chair
            { 3926, 2080 }, // Piano
            { 3927, 1442 }, // Knight statue
            { 3928, 1446 }, // Minotaur statue
            { 3929, 2034 }, // Large amphora
            { 3930, 1447 }, // Globin statue
            { 3931, 2101 }, // Indoor plant
            { 3932, 1770 }, // Barrel
            { 3933, 2105 }, // Christmas tree
            { 3934, 2117 }, // Rocking horse
            { 3935, 2582 }, // Telescope
            { 3936, 3832 }, // Bamboo drawer
            { 3937, 1775 }, // Through
            { 3938, 1750 }, // Trunk
            { 5086, 5056 }, // Monkey statue speak
            { 5087, 5055 }, // Monkey statue hear
            { 5088, 5046 }, // Monkey statue see
            { 6114, 5852 }, // Weapon rack
            { 6115, 6111 }, // Armor rack
            { 6372, 6357 }, // Oven
            { 6373, 6371 }, // Bookcase
            { 8692, 8688 }, // Chimney
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (constructionKits.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.Parent is Tile tile)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                    } );
                }

                Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.PutTheContructionKitOnTheFloorFirst) );

                return Promise.Break;
            }

            return next();
        }
    }
}