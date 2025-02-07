using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerSayCommand : Command
    {
        public PlayerSayCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            PlayerMuteBehaviour playerChannelMuteBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerMuteBehaviour>(Player);

            if (playerChannelMuteBehaviour != null)
            {
                string message;

                if (playerChannelMuteBehaviour.IsMuted(out message) )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, message) );

                    return Promise.Break;
                }      
            }

            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.Say, Player.Tile.Position, Message);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Player.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket);                    
                }
            }

            PlayerSayEventArgs e = new PlayerSayEventArgs(Player, Message);

            ObserveEventArgs<PlayerSayEventArgs> oe = ObserveEventArgs.Create(e);

            foreach (var npc in Context.Server.Map.GetObserversOfTypeNpc(Player.Tile.Position) )
            {
                if (npc.Tile.Position.CanSee(Player.Tile.Position) )
                {
                    Context.AddEvent(npc, oe);
                }
            }
              
            Context.AddEvent(Player, e);

            return Promise.Completed;
        }
    }
}