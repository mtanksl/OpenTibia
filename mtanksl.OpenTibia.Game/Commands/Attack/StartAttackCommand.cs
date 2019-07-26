using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class StartAttackCommand : Command
    {
        public StartAttackCommand(Player player, uint creatureId, uint nonce)
        {
            Player = player;

            CreatureId = creatureId;

            Nonce = nonce;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public uint Nonce { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Creature creature = server.Map.GetCreature(CreatureId);

            if (creature != null)
            {
                //Act

                Player.AttackTarget = creature;

                if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                {
                    Player.FollowTarget = null;
                }
                else
                {
                    Player.FollowTarget = creature;
                }

                //Notify

                base.Execute(server, context);
            }
        }
    }
}