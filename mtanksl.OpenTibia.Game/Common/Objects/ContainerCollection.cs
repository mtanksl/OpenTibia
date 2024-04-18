using OpenTibia.Common.Objects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Common
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

        /// <exception cref="InvalidOperationException"></exception>

        private byte GenerateContainerId()
        {
            for (byte containerId = 0; containerId < containers.Length; containerId++)
            {
                if (containers[containerId] == null)
                {
                    return containerId;
                }
            }

            throw new InvalidOperationException("Container limit exceeded.");
        }

        public byte OpenContainer(Container container)
        {
            byte containerId = GenerateContainerId();

            containers[containerId] = container;

            container.AddPlayer(client.Player);

            return containerId;
        }

        public void ReplaceContainer(Container newContainer, byte containerId)
        {
            Container oldContainer = GetContainer(containerId);

            oldContainer.RemovePlayer(client.Player);

            containers[containerId] = newContainer;

            newContainer.AddPlayer(client.Player);
        }

        public void CloseContainer(byte containerId)
        {
            Container container = GetContainer(containerId);

            containers[containerId] = null;

            container.RemovePlayer(client.Player);
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