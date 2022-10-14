using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (FightMode != Player.Client.FightMode)
                {
                    Player.Client.FightMode = FightMode;
                }

                if (ChaseMode != Player.Client.ChaseMode)
                {
                    Player.Client.ChaseMode = ChaseMode;

                    AttackAndFollowBehaviour component = context.Server.Components.GetComponent<AttackAndFollowBehaviour>(Player);

                    if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                    {
                        component.StopFollow();                        
                    }
                    else
                    {
                        component.StartFollow();
                    }
                }

                if (SafeMode != Player.Client.SafeMode)
                {
                    Player.Client.SafeMode = SafeMode;
                }

                resolve(context);
            } );
        }
    }
}