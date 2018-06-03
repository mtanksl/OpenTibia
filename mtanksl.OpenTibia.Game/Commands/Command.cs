using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {   
        public abstract void Execute(Context context);
    }
}