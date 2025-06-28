using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseLookInBattleListCommand : ParseLookCommand
    {
        public ParseLookInBattleListCommand(Player player, uint creatureId) : base(player)
        {
            CreatureId = creatureId;
        }

        public uint CreatureId { get; set; }

        public override Promise Execute()
        {
            Creature target = Context.Server.GameObjects.GetCreature(CreatureId);

            if (target != null && target != Player)
            {
                if (Player.Tile.Position.CanSee(target.Tile.Position) )
                {                                               
                    return Context.AddCommand(new PlayerLookCreatureCommand(Player, target) );
                }
            }

            throw new System.NotImplementedException();
        }
    }
}