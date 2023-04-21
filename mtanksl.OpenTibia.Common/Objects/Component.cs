namespace OpenTibia.Common.Objects
{
    public abstract class Component
    {
        public bool IsDestroyed { get; set; }

        public GameObject GameObject { get; set; }
    }
}