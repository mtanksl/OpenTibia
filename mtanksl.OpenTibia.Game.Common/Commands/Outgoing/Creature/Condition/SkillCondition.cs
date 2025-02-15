using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class SkillCondition : Condition
    {
        public SkillCondition( (Skill Skill, int ConditionSkill)[] skills, TimeSpan duration) : base(ConditionSpecialCondition.Skill)
        {
            Skills = skills;

            Duration = duration;
        }

        public (Skill Skill, int ConditionSkill)[] Skills { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override async Promise OnStart(Creature creature)
        {
            if (creature is Player player)
            {
                foreach (var item in Skills)
                {
                    await Context.Current.AddCommand(new PlayerUpdateSkillCommand(player, item.Skill, player.Skills.GetSkillPoints(item.Skill), player.Skills.GetSkillLevel(item.Skill), player.Skills.GetSkillPercent(item.Skill), item.ConditionSkill, player.Skills.GetItemSkillLevel(item.Skill) ) );
                }
            }
               
            await Promise.Delay(key, Duration);
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }

        public override async Promise OnStop(Creature creature)
        {
            if (creature is Player player)
            {
                foreach (var item in Skills)
                {
                    await Context.Current.AddCommand(new PlayerUpdateSkillCommand(player, item.Skill, player.Skills.GetSkillPoints(item.Skill), player.Skills.GetSkillLevel(item.Skill), player.Skills.GetSkillPercent(item.Skill), 0, player.Skills.GetItemSkillLevel(item.Skill) ) );
                }
            }

            await Promise.Completed;
        }
    }
}