using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class PlayerWrapItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerWrapItemCommand>(new WrapItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerWrapItemCommand>(new WrapItemChestHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerWrapItemCommand>(new WrapItemTransformHandler() );
        }

        public override void Stop()
        {

        }
    }   
}