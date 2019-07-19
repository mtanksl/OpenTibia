using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class ContainerCollection : IContainerCollection
    {
        private IClient client;

        public ContainerCollection(IClient client)
        {
            this.client = client;
        }

        private Dictionary<byte, Container> containers = new Dictionary<byte, Container>();

        private Dictionary<Container, byte> containerIds = new Dictionary<Container, byte>();

        public Container GetContainer(byte containerId)
        {
            Container container;

            containers.TryGetValue(containerId, out container);

            return container;
        }

        public bool IsOpen(Container container, out byte containerId)
        {
            return containerIds.TryGetValue(container, out containerId);
        }

        public byte Open(Container container)
        {
            byte containerId = GenerateId();

            containers.Add(containerId, container);

            containerIds.Add(container, containerId);

            return containerId;
        }

        public byte Close(Container container)
        {
            byte containerId;

            if (containerIds.TryGetValue(container, out containerId) )
            {
                containers.Remove(containerId);

                containerIds.Remove(container);
            }

            return containerId;
        }

        private byte GenerateId()
        {
            for (byte id = 0; id < 255; id++)
            {
                Container container;

                if ( !containers.TryGetValue(id, out container) )
                {
                    return id;
                }
            }

            throw new Exception();
        }
    }
}