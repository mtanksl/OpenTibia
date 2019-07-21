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

        private Container[] containers = new Container[255];

        private byte GenerateContainerId()
        {
            for (byte containerId = 0; containerId < containers.Length; containerId++)
            {
                if (containers[containerId] == null)
                {
                    return containerId;
                }
            }

            throw new Exception();
        }

        public byte OpenContainer(Container container)
        {
            byte containerId = GenerateContainerId();

            containers[containerId] = container;

            return containerId;
        }

        public void OpenContainer(byte containerId, Container container)
        {
            containers[containerId] = container;
        }

        public void CloseContainer(byte containerId)
        {
            containers[containerId] = null;
        }

        public Container GetContainer(byte containerId)
        {
            if (containerId < 0 || containerId > containers.Length - 1)
            {
                return null;
            }

            return containers[containerId];
        }

        public IEnumerable<Container> GetContainers()
        {
            foreach (var container in containers)
            {
                if (container != null)
                {
                    yield return container;
                }
            }
        }

        public IEnumerable< KeyValuePair<byte, Container> > GetIndexedContainers()
        {
            for (byte containerId = 0; containerId < containers.Length; containerId++)
            {
                if (containers[containerId] != null)
                {
                    yield return new KeyValuePair<byte, Container>(containerId, containers[containerId] );
                }
            }
        }
    }
}