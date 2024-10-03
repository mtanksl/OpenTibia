using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerYellCommand : Command
    {
        public PlayerYellCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            if (Player.Level == 1)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, "You may not yell as long as you are on level 1.") );

                return Promise.Break;
            }

            PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(Player);

            if (playerCooldownBehaviour != null)
            {
                if (playerCooldownBehaviour.HasCooldown("Yell") )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );

                    return Promise.Break;
                }

                playerCooldownBehaviour.AddCooldown("Yell", TimeSpan.FromSeconds(30) );
            }

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

            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.Yell, Player.Tile.Position, Message.ToUpper() );

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(Player.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearYell(Player.Tile.Position) )
                {
                    if (observer is Player player)
                    {
                        Context.AddPacket(player, showTextOutgoingPacket);
                    }
                                     
                    Context.AddEvent(observer, new PlayerYellEventArgs(Player, Message) );
                }
            }

            Context.AddEvent(new PlayerYellEventArgs(Player, Message) );

            return Promise.Completed;
        }
    }
}