using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class Skills
    {
        public Skills(Player player)
        {
            this.player = player;
        }

        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
        }

        private ulong[] skillPoints = new ulong[8];

        public ulong GetSkillPoints(Skill skill)
        {
            return skillPoints[ (byte)skill];
        }

        public void SetSkillPoints(Skill skill, ulong skillPoint)
        {
            skillPoints[ (byte)skill] = skillPoint;
        }

        private byte[] skillLevels = new byte[] { 0, 10, 10, 10, 10, 10, 10, 10 };

        public byte GetSkillLevel(Skill skill)
        {
            return skillLevels[ (byte)skill];
        }

        public void SetSkillLevel(Skill skill, byte skillLevel)
        {
            skillLevels[ (byte)skill] = skillLevel;
        }

        private byte[] skillPercents = new byte[8];
                        
        public byte GetSkillPercent(Skill skill)
        {
            return skillPercents[ (byte)skill];
        }

        public void SetSkillPercent(Skill skill, byte skillPercent)
        {
            skillPercents[ (byte)skill] = skillPercent;
        }

        private int[] conditionSkillLevel = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public int GetConditionSkillLevel(Skill skill)
        {
            return conditionSkillLevel[ (byte)skill];
        }

        public void SetConditionSkillLevel(Skill skill, int skillModifier)
        {
            conditionSkillLevel[ (byte)skill] = skillModifier;
        }

        private int[] itemSkillLevel = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public int GetItemSkillLevel(Skill skill)
        {
            return itemSkillLevel[ (byte)skill];
        }

        public void SetItemSkillLevel(Skill skill, int skillModifier)
        {
            itemSkillLevel[ (byte)skill] = skillModifier;
        }

        public byte GetClientSkillLevel(Skill skill)
        {
            return (byte)(skillLevels[ (byte)skill ] + conditionSkillLevel[ (byte)skill] + itemSkillLevel[ (byte)skill] );
        }
    }
}