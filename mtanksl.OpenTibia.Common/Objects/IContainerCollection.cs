namespace OpenTibia.Common.Objects
{
    public interface IContainerCollection
    {
        Container GetContainer(byte containerId);

        bool HasContainer(Container container, out byte containerId);

        byte OpenContainer(Container container);

        byte CloseContainer(Container container);
    }
}