using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CombatControlsCommand : Command
    {
        public CombatControlsCommand(Player player, FightMode fightMode, ChaseMode chaseMode, SafeMode safeMode)
        {
            Player = player;

            FightMode = fightMode;

            ChaseMode = chaseMode;

            SafeMode = safeMode;
        }

        public Player Player { get; set; }

        public FightMode FightMode { get; set; }

        public ChaseMode ChaseMode { get; set; }

        public SafeMode SafeMode { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            Player.Client.FightMode = FightMode;

            Player.Client.ChaseMode = ChaseMode;

            Player.Client.SafeMode = SafeMode;

            //Notify

            base.Execute(server, context);
        }
    }
}