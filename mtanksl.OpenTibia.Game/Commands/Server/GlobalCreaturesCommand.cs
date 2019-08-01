using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class GlobalCreaturesCommand : Command
    {
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            foreach (var creature in server.Map.GetCreatures() )
            {
                foreach (var component in creature.GetComponents<IBehaviour>() )
                {
                    component.Update(server, context);
                }
            }

            //Notify

            server.QueueForExecution(Constants.GlobalCreaturesSchedulerEvent, Constants.GlobalCreaturesSchedulerEventInterval, this);
        }
    }
}