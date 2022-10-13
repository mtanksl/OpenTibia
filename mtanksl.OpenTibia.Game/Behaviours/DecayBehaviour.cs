using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class DecayBehaviour : Behaviour
    {
        private int executeInMilliseconds;

        public DecayBehaviour(int executeInMilliseconds)
        {
            this.executeInMilliseconds = executeInMilliseconds;
        }

        private Item item;

        private string key;

        private Promise promise;

        public Promise Promise
        {
            get
            {
                return promise;
            }
        }

        public override void Start(Server server)
        {
            item = (Item)GameObject;

            key = "Decay" + item.Id;

            promise = Promise.Delay(server, key, executeInMilliseconds).Then(ctx =>
            {
                GameObject.RemoveComponent(this);
            } );
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}