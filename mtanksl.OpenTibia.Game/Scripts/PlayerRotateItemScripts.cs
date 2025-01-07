using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class PlayerRotateItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerRotateItemCommand>(new RotateItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerRotateItemCommand>(new RotateItemChestHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerRotateItemCommand>(new RotateItemTransformHandler() );
        }

        public override void Stop()
        {

        }
    }   
}