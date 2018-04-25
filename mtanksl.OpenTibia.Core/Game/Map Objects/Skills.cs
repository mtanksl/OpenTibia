namespace OpenTibia
{
    public class Skills
    {
        private byte[] values = new byte[8];

        private byte[] percents = new byte[8];

        public Skills()
        {
            for (int index = 1; index < 8; index++)
            {
                values[index] = 10;
            }
        }

        public byte GetValue(Skill skill)
        {
            return values[ (int)skill ];
        }

        public void SetValue(Skill skill, byte value)
        {
            values[ (int)skill ] = value;
        }

        public byte GetPercent(Skill skill)
        {
            return percents[ (int)skill ];
        }

        public void SetPercent(Skill skill, byte percent)
        {
            percents[ (int)skill ] = percent;
        }
    }
}