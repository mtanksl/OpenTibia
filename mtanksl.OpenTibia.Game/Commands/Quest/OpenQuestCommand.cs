using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class OpenQuestCommand : Command
    {
        public OpenQuestCommand(Player player, ushort questId)
        {
            Player = player;

            QuestId = questId;
        }

        public Player Player { get; set; }

        public ushort QuestId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}