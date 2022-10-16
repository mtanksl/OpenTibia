using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureUpdateParentScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MoveAwayContainerCloseHandler() );        

            server.CommandHandlers.Add(new MagicForcefieldHandler() );

            server.CommandHandlers.Add(new HoleHandler() );

            server.CommandHandlers.Add(new PitfallHandler() );

            server.CommandHandlers.Add(new StairsHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}