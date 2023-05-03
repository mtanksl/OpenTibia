using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components.Conversations
{
    public class ParametersUpdate : TopicCallback
    {
        private object parameters;

        public ParametersUpdate(object parameters)
        {
            this.parameters = parameters;
        }

        public override Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            var obj = parameters;

            foreach (var property in obj.GetType().GetProperties() )
            {
                conversation.Data[property.Name] = property.GetValue(obj);
            }

            return Promise.Completed;
        }
    }
}