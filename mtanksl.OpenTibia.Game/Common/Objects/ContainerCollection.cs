using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class ContainerCollection : IContainerCollection
    {
        public ContainerCollection(IClient client)
        {
            this.client = client;
        }

        private IClient client;

        private IClient Client
        {
            get
            {
                return client;
            }
        }

        private Dictionary<byte, Container> containers = new Dictionary<byte, Container>();

        private Dictionary<Container, byte> containerIds = new Dictionary<Container, byte>();

        private byte GenerateId()
        {
            for (byte id = 0; id < 255; id++)
            {
                if ( !containers.ContainsKey(id) )
                {
                    return id;
                }
            }

            throw new Exception();
        }

        public Container GetContainer(byte containerId)
        {
            Container container;

            containers.TryGetValue(containerId, out container);

            return container;
        }

        public bool HasContainer(Container container, out byte containerId)
        {
            return containerIds.TryGetValue(container, out containerId);
        }

        public byte OpenContainer(Container container)
        {
            byte containerId = GenerateId();

            containers.Add(containerId, container);

            containerIds.Add(container, containerId);

            return containerId;
        }

        public byte CloseContainer(Container container)
        {
            byte containerId;

            if ( HasContainer(container, out containerId) )
            {
                containers.Remove(containerId);

                containerIds.Remove(container);
            }

            return containerId;
        }
    }
}