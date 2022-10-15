using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemUpdateParentToTileScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MagicForcefield2Handler() );

            server.CommandHandlers.Add(new Hole2Handler() );

            server.CommandHandlers.Add(new Stairs2Handler() );

            server.CommandHandlers.Add(new CandlestickMoveHandler() );

            server.CommandHandlers.Add(new TrapMoveHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}