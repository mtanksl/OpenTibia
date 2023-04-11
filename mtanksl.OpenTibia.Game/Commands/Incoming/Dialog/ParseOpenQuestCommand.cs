using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenQuestCommand : Command
    {
        public ParseOpenQuestCommand(Player player, ushort questId)
        {
            Player = player;

            QuestId = questId;
        }

        public Player Player { get; set; }

        public ushort QuestId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {


                resolve();
            } );
        }
    }
}