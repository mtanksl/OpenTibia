using System;

namespace OpenTibia.Game.Commands
{
    public class YieldCommand : Command
    {
        public YieldCommand()
        {
            
        }
        
        public override Promise Execute(Context context)
        {
            return Promise.Yield(context);
        }
    }
}