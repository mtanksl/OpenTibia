using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLoginCommand : Command
    {
        public PlayerLoginCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Context.AddPacket(Player, new SendInfoOutgoingPacket(Player.Id, Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster) );

            Context.AddPacket(Player, new SendTilesOutgoingPacket(Context.Server.Map, Player.Client, Player.Tile.Position) );

            Context.AddPacket(Player, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );
                                
            Context.AddPacket(Player, new SendStatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.MagicLevel, Player.Skills.MagicLevelPercent, Player.Soul, Player.Stamina) );

            Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );

            Context.AddPacket(Player, new SetSpecialConditionOutgoingPacket(Player.SpecialConditions) );

            foreach (var pair in Player.Inventory.GetIndexedContents() )
            {
                Context.AddPacket(Player, new SlotAddOutgoingPacket( (byte)pair.Key, (Item)pair.Value) );
            }

            foreach (var pair in Player.Client.Vips.GetIndexed() )
            {
                Context.AddPacket(Player, new VipOutgoingPacket( (uint)pair.Key, pair.Value, false) );
            }

            Context.AddEvent(new PlayerLoginEventArgs(Player.Tile, Player) );

            return Promise.Completed;
        }
    }
}