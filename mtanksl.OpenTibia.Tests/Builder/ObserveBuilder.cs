using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Tests
{
    public class ObserveBuilder : IObserveBuilder
    {
        private IServer server;

        private Action<IObserveBuilder> options;

        public ObserveBuilder(IServer server, IConnection connection, Action<IObserveBuilder> options)
        {
            this.server = server;

            this.options = options;

            this.connection = connection;
        }

        private IConnection connection;

        public IConnection Connection
        {
            get
            {
                return connection;
            }
        }

        private int? total;

        public IObserveBuilder ExpectPacket(int total)
        {
            this.total = total;

            return this;
        }

        private List<Assertion> assertions = new List<Assertion>();

        public IObserveBuilder ExpectPacket<TPacket>(int total) where TPacket : IOutgoingPacket
        {
            this.assertions.Add(new Assertion()
            {
                Type = typeof(TPacket),
                
                Total = total
            } );

            return this;
        }

        public IObserveBuilder ExpectPacket<TPacket>(int total, Func<TPacket, bool> callback) where TPacket : IOutgoingPacket
        {
            this.assertions.Add(new Assertion()
            {
                Type = typeof(TPacket),
                
                Total = total, 
                
                Callback = packet => callback( (TPacket)packet) 
            } );

            return this;
        }

        private bool? connected;

        public IObserveBuilder ExpectConnected(bool connected = true)
        {
            this.connected = connected;

            return this;
        }

        public void InternalRunBegin()
        {
            server.QueueForExecution( () =>
            {
                options(this);

                return Promise.Completed;

            } ).Wait();

            ( (MockConnection)connection).BeginRecord();
        }

        public void InternalRunEnd()
        {
            var outgoingPackets = ( (MockConnection)connection).EndRecord();

            if (total != null)
            {
                Assert.AreEqual(total.Value, outgoingPackets.Count(), "Unexpected packet total.");
            }

            foreach (var assertion in assertions)
            {
                if (assertion.Callback == null)
                {
                    Assert.AreEqual(assertion.Total, outgoingPackets.Where(o => o.GetType() == assertion.Type).Count(), "Unexpected packet " + assertion.Type.Name + " total.");
                }
                else
                {
                    Assert.AreEqual(assertion.Total, outgoingPackets.Where(o => o.GetType() == assertion.Type && assertion.Callback(o) ).Count(), "Unexpected packet " + assertion.Type.Name + " total.");
                }
            }

            if (connected != null)
            {
                Assert.AreEqual(connected.Value, !( (MockConnection)connection).IsDisconnected, "Unexpected connection status.");
            }
        }
    }
}