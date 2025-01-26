using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class CreatureAttackCreatureCommand : Command
    {
        public CreatureAttackCreatureCommand(Creature attacker, Creature target, Attack attack)

            : this(attacker, target, attack, null)
        {
           
        }

        public CreatureAttackCreatureCommand(Creature attacker, Creature target, Attack attack, Condition condition)
        {
            Attacker = attacker;

            Target = target;

            Attack = attack;

            Condition = condition;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public Attack Attack { get; set; }

        public Condition Condition { get; set; }

        public override async Promise Execute()
        {
            int damage = Attack.Calculate(Attacker, Target);

            if (damage == 0)
            {
                await Attack.Missed(Attacker, Target);
            }
            else
            {
                await Attack.Hit(Attacker, Target, damage);

                if (Condition != null)
                {
                    await Context.AddCommand(new CreatureAddConditionCommand(Target, Condition) );
                }
            }

            if (Attack is DamageAttack)
            {
                if (Attacker is Player attacker1 && Target is Player target1)
                {
                    ICombatCollection combats = Context.Server.Combats;

                    if ( !combats.WhiteSkullContains(attacker1) ) // If an attacker without white skull
                    {
                        if ( !combats.WhiteSkullContains(target1) ) // Attacks a target without white skull
                        {
                            combats.WhiteSkullAdd(attacker1, target1); // Makes attacker white skull, and add target to self defense list

                            await Context.Current.AddCommand(new CreatureAddConditionCommand(attacker1, new ProtectionZoneBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayLogoutBlockSeconds) ) ) );

                            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(attacker1.Tile.Position) )
                            {
                                byte clientIndex;

                                if (observer.Client.TryGetIndex(attacker1, out clientIndex) )
                                {
                                    Context.AddPacket(observer, new SetSkullIconOutgoingPacket(attacker1.Id, SkullIcon.White) );
                                }
                            }
                        }
                        else // Attacks a target with white skull
                        {
                            if (combats.WhiteSkullContains(target1, attacker1)) // If it is self defense
                            {
                                await Context.Current.AddCommand(new CreatureAddConditionCommand(attacker1, new LogoutBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayLogoutBlockSeconds) ) ) );
                            }
                            else // If it is not self defense
                            {
                                combats.YellowSkullAdd(attacker1, target1); // Makes attacker yellow skull

                                await Context.Current.AddCommand(new CreatureAddConditionCommand(attacker1, new ProtectionZoneBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayLogoutBlockSeconds) ) ) );

                                Context.AddPacket(target1, new SetSkullIconOutgoingPacket(attacker1.Id, SkullIcon.Yellow) );
                            }
                        }
                    }
                    else // If an attacker with white skull
                    {
                        if ( !combats.WhiteSkullContains(target1) && 
                             !combats.WhiteSkullContains(attacker1, target1) && 
                             !combats.YellowSkullContains(target1, attacker1) ) // Attacks a new target without white skull
                        {
                            combats.WhiteSkullAdd(attacker1, target1); // Add target to self defense list
                        }
                        
                        await Context.Current.AddCommand(new CreatureAddConditionCommand(attacker1, new ProtectionZoneBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayLogoutBlockSeconds) ) ) );
                    }
                   
                    await Context.Current.AddCommand(new CreatureAddConditionCommand(target1, new LogoutBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayLogoutBlockSeconds) ) ) );
                }
                else if (Attacker is Player attacker2)
                {
                    await Context.Current.AddCommand(new CreatureAddConditionCommand(attacker2, new LogoutBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayLogoutBlockSeconds) ) ) );
                }
                else if (Target is Player target2)
                {                    
                    await Context.Current.AddCommand(new CreatureAddConditionCommand(target2, new LogoutBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayLogoutBlockSeconds) ) ) );
                }
            }
        }
    }
}