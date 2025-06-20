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

        GameChat = 32,

        Trade = 64,

        TradeRookgaard = 128,

        RealLifeChat = 256,

        Help = 512,

        PrivateChannel = 1024
    }

    public static class ChannelFlagsExtensions
    {
        public static bool Is(this ChannelFlags flags, ChannelFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}