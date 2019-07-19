namespace OpenTibia.Common.Objects
{
    public interface IContainerCollection
    {
        Container GetContainer(byte containerId);

        bool IsOpen(Container container, out byte containerId);

        byte Open(Container container);

        byte Close(Container container);
    }
}