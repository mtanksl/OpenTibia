using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemTransformHandler : CommandHandler<PlayerRotateItemCommand>
    {
        private static Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
             // Throne
            { 1647, 1648 },
            { 1648, 1649 },
            { 1649, 1646 },
            { 1646, 1647 },

            // Wooden chair
            { 1652, 1653 },
            { 1653, 1650 },
            { 1650, 1651 },
            { 1651, 1652 },

             // Throne
            { 1654, 1657 },
            { 1657, 1655 },
            { 1655, 1656 },
            { 1656, 1654 },

            // Sofa chair
            { 1658, 1661 },
            { 1661, 1659 },
            { 1659, 1660 },
            { 1660, 1658 },

            // Red cushioned chair
            { 1666, 1669 },
            { 1669, 1667 },
            { 1667, 1668 },
            { 1668, 1666 },

            // Green cushioned chair
            { 1670, 1673 },
            { 1673, 1671 },
            { 1671, 1672 },
            { 1672, 1670 },

            // Rocking chair
            { 1674, 1677 },
            { 1677, 1675 },
            { 1675, 1676 },
            { 1676, 1674 },

            // Drawers
            { 1716, 1715 },
            { 1715, 1714 },
            { 1714, 1717 },
            { 1717, 1716 },

            // Dresser
            { 1724, 1727 },
            { 1727, 1726 },
            { 1726, 1725 },
            { 1725, 1724 },

            // Pendumum clock
            { 1728, 1731 },
            { 1731, 1730 },
            { 1730, 1729 },
            { 1729, 1728 },

            // Locker
            { 1732, 1735 },
            { 1735, 1733 },
            { 1733, 1734 },
            { 1734, 1732 },

            // Mirror
            { 1736, 1737 },
            { 1737, 1736 },

            // Chest
            { 1740, 1749 },
            { 1749, 1747 },
            { 1747, 1748 },
            { 1748, 1740 },

            // Large trunk
            { 1750, 1753 },
            { 1753, 1751 },
            { 1751, 1752 },
            { 1752, 1750 },

            // Rocking horse
            { 2117, 2118 },
            { 2118, 2119 },
            { 2119, 2116 },
            { 2116, 2117 },

            // Piano
            { 2080, 2083 },
            { 2083, 2081 },
            { 2081, 2082 },
            { 2082, 2080 },

            // Harp
            { 2084, 2085 },
            { 2085, 2084 },

            // Stone table
            { 3805, 3806 },
            { 3806, 3805 },

            // Tusk table
            { 3807, 3808 },
            { 3808, 3807 },

            // Bamboo table
            { 3809, 3810 },
            { 3810, 3809 },

            // Tusk chair
            { 3813, 3816 },
            { 3816, 3814 },
            { 3814, 3815 },
            { 3815, 3813 },

            // Ivory chair
            { 3817, 3820 },
            { 3820, 3818 },
            { 3818, 3819 },
            { 3819, 3817 },

             // Oven
            { 6356, 6358 },
            { 6358, 6360 },
            { 6360, 6362 },
            { 6362, 6356 },

            { 6357, 6359 },
            { 6359, 6361 },
            { 6361, 6363 },
            { 6363, 6357 },

            // Bookcase
            { 6368, 6369 },
            { 6369, 6370 },
            { 6370, 6371 },
            { 6371, 6368 },

            //TODO: More items
        };

        public override Promise Handle(Func<Promise> next, PlayerRotateItemCommand command)
        {
            ushort toOpenTibiaId;

            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next();
        }
    }
}