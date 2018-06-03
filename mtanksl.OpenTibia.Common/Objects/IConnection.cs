using System;

namespace OpenTibia.Common.Objects
{
    public interface IConnection 
    {
        IClient Client { get; set; }

        uint[] Keys { get; set; }

        DateTime Latency { get; set; }

        void Send(byte[] bytes);

        void Disconnect();
    }
}