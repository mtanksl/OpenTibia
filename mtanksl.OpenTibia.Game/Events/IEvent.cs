using OpenTibia.Web;

namespace OpenTibia.Game.Events
{
    public interface IEvent
    {
        void Execute(Context context);
    }
}