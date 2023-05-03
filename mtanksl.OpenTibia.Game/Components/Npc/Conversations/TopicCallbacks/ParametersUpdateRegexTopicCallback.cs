using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components.Conversations
{
    public class ParametersUpdateRegex<T> : TopicCallback
    {
        private Func<T, object> parameters;

        public ParametersUpdateRegex(Func<T, object> parameters)
        {
            this.parameters = parameters;
        }

        public override Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            var obj = parameters( (T)conversation.Data["Group"] );

            foreach (var property in obj.GetType().GetProperties() )
            {
                conversation.Data[property.Name] = property.GetValue(obj);
            }

            return Promise.Completed;
        }
    }
}