namespace OpenTibia.Common.Objects
{
    public abstract class Component
    {
        public bool Canceled { get; set; }

        public GameObject GameObject { get; set; }
    }
}