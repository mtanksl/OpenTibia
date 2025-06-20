using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    [Flags]
    public enum ChannelFlags : ushort
    {
        None = 0,

        Guild = 1,

        Party = 2,

        Tutor = 4,

        RuleViolations = 8,

        Gamemaster = 16,

        Trade = 32,

        TradeRookgaard = 64,

        Help = 128,

        PrivateChannel = 256
    }

    public static class ChannelFlagsExtensions
    {
        public static bool Is(this ChannelFlags flags, ChannelFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}