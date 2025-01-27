using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CleanUpCombatCollectionHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    player.Combat.Clear();

                    foreach (var observer in Context.Current.Server.Map.GetObserversOfTypePlayer(player.Tile.Position) )
                    {
                        byte clientIndex;

                        if (observer.Client.TryGetIndex(player, out clientIndex) )
                        {
                            Context.Current.AddPacket(observer, new SetSkullIconOutgoingPacket(player.Id, observer.Client.GetSkullIcon(player) ) );
                        }
                    }

                    return Promise.Completed;
                } );
            }

            return next();            
        }
    }
}