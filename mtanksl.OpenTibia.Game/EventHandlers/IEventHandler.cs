using OpenTibia.Web;
using System;

namespace OpenTibia.Game.EventHandlers
{
    public interface IEventHandler
    {
        void Execute(EventArgs e, Context context);
    }
}