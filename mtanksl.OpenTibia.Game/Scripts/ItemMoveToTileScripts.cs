using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class ItemMoveToTileScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MagicForcefield2Handler() );

            server.CommandHandlers.Add(new Hole2Handler() );

            server.CommandHandlers.Add(new Stairs2Handler() );

            server.CommandHandlers.Add(new DustbinHandler() );

            server.CommandHandlers.Add(new ShallowWaterHandler() );

            server.CommandHandlers.Add(new SwampHandler() );

            server.CommandHandlers.Add(new LavaHandler() );

            server.CommandHandlers.Add(new CandlestickHandler() );

            server.CommandHandlers.Add(new TrapHandler() );

            server.CommandHandlers.Add(new MoveItemToTileContainerCloseHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}