using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureWalkScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new MoveAwayContainerCloseHandler() );

            Context.Server.CommandHandlers.Add(new MagicForcefieldHandler() );

            Context.Server.CommandHandlers.Add(new HoleHandler() );

            Context.Server.CommandHandlers.Add(new PitfallHandler() );

            Context.Server.CommandHandlers.Add(new StairsHandler() );

            Context.Server.CommandHandlers.Add(new SwimHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}