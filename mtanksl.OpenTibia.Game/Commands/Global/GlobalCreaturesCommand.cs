using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class GlobalCreaturesCommand : Command
    {
        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            foreach (var creature in server.Map.GetCreatures() )
            {
                foreach (var component in creature.GetComponents<Behaviour>() )
                {
                    component.Update(server, context);
                }
            }

            server.QueueForExecution(Constants.GlobalCreaturesSchedulerEvent, Constants.GlobalCreaturesSchedulerEventInterval, this);

            //Notify
        }
    }
}