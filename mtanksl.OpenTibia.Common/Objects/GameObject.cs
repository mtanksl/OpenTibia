namespace OpenTibia.Common.Objects
{
    public abstract class GameObject
    {
        public bool IsDestroyed { get; set; }

        public uint Id { get; set; }
    }
}