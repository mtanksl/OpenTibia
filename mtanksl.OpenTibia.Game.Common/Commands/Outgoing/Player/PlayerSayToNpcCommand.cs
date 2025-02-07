using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Commands
{
    public class PlayerSayToNpcCommand : Command
    {
        public PlayerSayToNpcCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            PlayerSayToNpcEventArgs e = new PlayerSayToNpcEventArgs(Player, Message);

            foreach (var npc in Context.Server.Map.GetObserversOfTypeNpc(Player.Tile.Position) )
            {
                if (npc.Tile.Position.CanSee(Player.Tile.Position) )
                {
                    Context.AddEvent(npc, ObserveEventArgs.Create(npc, e) );
                }
            }
               
            Context.AddEvent(Player, e);

            return Promise.Completed;
        }
    }
}