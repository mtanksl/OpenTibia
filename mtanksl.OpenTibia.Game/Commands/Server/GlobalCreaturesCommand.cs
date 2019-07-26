namespace OpenTibia.Game.Commands
{
    public class GlobalCreaturesCommand : Command
    {
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            foreach (var monster in server.Map.GetMonsters() )
            {

            }

            //Notify

            server.QueueForExecution(Constants.GlobalCreaturesSchedulerEvent, 1000, this);
        }
    }
}