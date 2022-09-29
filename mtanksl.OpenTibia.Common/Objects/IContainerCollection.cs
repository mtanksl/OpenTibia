using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IContainerCollection
    {
        byte OpenContainer(Container container);

        void ReplaceContainer(Container container, byte containerId);

        void CloseContainer(byte containerId);

        Container GetContainer(byte containerId);

        IEnumerable<Container> GetContainers();

        IEnumerable<KeyValuePair<byte, Container> > GetIndexedContainers();
    }
}