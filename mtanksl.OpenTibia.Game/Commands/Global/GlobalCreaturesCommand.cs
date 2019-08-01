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
                    if ( !component.Enabled )
                    {
                        component.Enabled = true;

                        component.Start(server);
                    }

                    component.Update(server, context);
                }
            }

            //Notify

            server.QueueForExecution(Constants.GlobalCreaturesSchedulerEvent, Constants.GlobalCreaturesSchedulerEventInterval, this);
        }
    }
}