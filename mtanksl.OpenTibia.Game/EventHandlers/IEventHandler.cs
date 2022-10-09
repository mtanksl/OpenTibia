using System;

namespace OpenTibia.Game.EventHandlers
{
    public interface IEventHandler
    {
        Guid Token { get; }

        void Handle(Context context, object e);
    }
}