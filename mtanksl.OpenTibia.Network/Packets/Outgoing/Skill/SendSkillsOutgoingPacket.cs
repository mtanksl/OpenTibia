using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendSkillsOutgoingPacket : IOutgoingPacket
    {
        public SendSkillsOutgoingPacket(Skills skills)
        {
            this.Skills = skills;
        }

        public Skills Skills { get; set; }
                
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA1 );

            foreach (var skill in new[] { Skill.Fist, Skill.Club, Skill.Sword, Skill.Axe, Skill.Distance, Skill.Shield, Skill.Fish } )
            {
                if ( !features.HasFeatureFlag(FeatureFlag.SkillLevelU16) )
                {
                    writer.Write( (byte)Skills.GetClientSkillLevel(skill) );
                }
                else
                {
                    writer.Write(Skills.GetClientSkillLevel(skill) );
                }

                if (features.HasFeatureFlag(FeatureFlag.PlayerSkillsBase) )
                {
                    if ( !features.HasFeatureFlag(FeatureFlag.SkillLevelU16) )
                    {
                        writer.Write( (byte)Skills.GetSkillLevel(skill) );
                    }
                    else
                    {
                        writer.Write(Skills.GetSkillLevel(skill) );
                    }                    
                }

                writer.Write(Skills.GetSkillPercent(skill) );
            }

            if (features.HasFeatureFlag(FeatureFlag.AdditionalSkills) )
            {
                for (int i = 0; i < 6; i++)
                {
                    if ( !features.HasFeatureFlag(FeatureFlag.SkillLevelU16) )
                    {
                        writer.Write( (byte)0);
                    }
                    else
                    {
                        writer.Write( (ushort)0);
                    }

                    if (features.HasFeatureFlag(FeatureFlag.PlayerSkillsBase) )
                    {
                        if ( !features.HasFeatureFlag(FeatureFlag.SkillLevelU16) )
                        {
                            writer.Write( (byte)0);
                        }
                        else
                        {
                            writer.Write( (ushort)0);
                        }                    
                    }
                }
            }
        }
    }
}