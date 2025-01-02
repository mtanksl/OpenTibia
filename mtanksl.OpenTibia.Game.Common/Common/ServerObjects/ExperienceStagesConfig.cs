namespace OpenTibia.Game.Common.ServerObjects
{
    public class ExperienceStagesConfig
    {
        public bool Enabled { get; set; }

        public LevelConfig[] Levels { get; set; }
    }

    public class LevelConfig
    {
        public int MinLevel { get; set; }

        public int MaxLevel { get; set; }

        public int Multiplier { get; set; }
    }
}