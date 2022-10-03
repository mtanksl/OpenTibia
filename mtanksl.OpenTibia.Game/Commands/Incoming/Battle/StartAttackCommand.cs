﻿using OpenTibia.Common.Objects;
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

        public override void Execute(Context context)
        {
            Creature creature = context.Server.GameObjects.GetGameObject<Creature>(CreatureId);

            if (creature != null && creature != Player)
            {
                Player.AttackTarget = creature;

                if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                {
                    Player.FollowTarget = null;
                }
                else
                {
                    Player.FollowTarget = creature;
                }
            }

            OnComplete(context);
        }
    }
}