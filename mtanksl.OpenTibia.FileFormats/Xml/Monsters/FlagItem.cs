namespace OpenTibia.FileFormats.Xml.Monsters
{
    // This format does not make much sense. Only need 1 node with multiple attributes.

    public class FlagItem
    {
        public int? Summonable { get; set; }

        public int? Attackable { get; set; }

        public int? Hostile { get; set; }

        public int? Illusionable { get; set; }

        public int? Convinceable { get; set; }

        public int? Pushable { get; set; }

        public int? CanPushItems { get; set; }

        public int? CanPushCreatures { get; set; }

        public int? TargetDistance { get; set; }

        public int? RunOnHealth { get; set; }
    }
}