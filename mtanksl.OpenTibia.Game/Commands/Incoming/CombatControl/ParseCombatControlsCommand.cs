using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ParseCombatControlsCommand : Command
    {
        public ParseCombatControlsCommand(Player player, FightMode fightMode, ChaseMode chaseMode, SafeMode safeMode)
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

        public override Promise Execute()
        {
            if (FightMode != Player.Client.FightMode)
            {
                Player.Client.FightMode = FightMode;
            }

            if (ChaseMode != Player.Client.ChaseMode)
            {
                Player.Client.ChaseMode = ChaseMode;

                PlayerAttackAndFollowBehaviour playerAttackAndFollowBehaviour = Context.Server.Components.GetComponent<PlayerAttackAndFollowBehaviour>(Player);

                if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                {
                    if (playerAttackAndFollowBehaviour != null)
                    {
                        playerAttackAndFollowBehaviour.StopFollow();                        
                    }
                }
                else
                {
                    if (playerAttackAndFollowBehaviour != null)
                    {
                        playerAttackAndFollowBehaviour.StartFollow();
                    }
                }
            }

            if (SafeMode != Player.Client.SafeMode)
            {
                Player.Client.SafeMode = SafeMode;
            }

            return Promise.Completed;
        }
    }
}