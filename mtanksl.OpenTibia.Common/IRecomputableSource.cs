using System;

namespace OpenTibia.Common.Objects
{
    public interface IRecomputableSource
    {
        event EventHandler Changed;
    }
}