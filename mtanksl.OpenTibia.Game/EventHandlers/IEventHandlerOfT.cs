using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.EventHandlers
{
    public interface IEventHandler<T> : IEventHandler where T : GameEventArgs
    {
        Promise Handle(T e);
    }
}