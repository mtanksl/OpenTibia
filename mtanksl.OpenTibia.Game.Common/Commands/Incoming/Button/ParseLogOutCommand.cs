using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseLogOutCommand : IncomingCommand
    {
        public ParseLogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Player.HasSpecialCondition(SpecialCondition.NoLogoutZone) )
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouMayNotLogOutHere) );

                return Promise.Break;
            }

            if (Player.HasSpecialCondition(SpecialCondition.LogoutBlock) || Player.HasSpecialCondition(SpecialCondition.ProtectionZoneBlock) )
            {
                if ( !Player.Tile.ProtectionZone || Player.HasSpecialCondition(SpecialCondition.Poisoned) || Player.HasSpecialCondition(SpecialCondition.Burning) || Player.HasSpecialCondition(SpecialCondition.Electrified) || Player.HasSpecialCondition(SpecialCondition.Drowning) || Player.HasSpecialCondition(SpecialCondition.Freezing) || Player.HasSpecialCondition(SpecialCondition.Dazzled) || Player.HasSpecialCondition(SpecialCondition.Cursed) || Player.HasSpecialCondition(SpecialCondition.Bleeding) )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouMayNotLogoutDuringOrImmediatelyAfterAFight) );

                    return Promise.Break;
                }
            }

            return Context.AddCommand(new ShowMagicEffectCommand(Player, MagicEffectType.Puff) ).Then( () =>
            {
                return Context.AddCommand(new CreatureDestroyCommand(Player) );
            } );
        }
    }
}