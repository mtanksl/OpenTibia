using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class ConversationStrategyBuilder
    {
        private string greetingMessage;

        public ConversationStrategyBuilder WithGreeting(string message)
        {
            this.greetingMessage = message;

            return this;
        }

        private string busyMessage;

        public ConversationStrategyBuilder WithBusy(string message)
        {
            this.busyMessage = message;

            return this;
        }

        private Dictionary<string, string> sayMessage = new Dictionary<string, string>();

        public ConversationStrategyBuilder WithSay(string question, string answer)
        {
            sayMessage.Add(question, answer);

            return this;
        }

        private string farewellMessage;

        public ConversationStrategyBuilder WithFarewell(string message)
        {
            this.farewellMessage = message;

            return this;
        }

        private string dismissMessage;

        public ConversationStrategyBuilder WithDismiss(string message)
        {
            this.dismissMessage = message;

            return this;
        }

        public ConversationStrategy Build()
        {
            return new ConversationStrategy()
            {
                GreetingMessage = greetingMessage,

                BusyMessage = busyMessage,

                SayMessage = sayMessage,

                FarewellMessage = farewellMessage,

                DismissMessage = dismissMessage
            };
        }
    }
}