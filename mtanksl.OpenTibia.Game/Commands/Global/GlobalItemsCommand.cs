using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class GlobalItemsCommand : Command
    {
        public override void Execute(Context context)
        {
            foreach (var item in context.Server.GameObjects.GetItems() )
            {
                foreach (var component in item.GetComponents<TimeBehaviour>() )
                {
                    component.Update(context);
                }
            }

            context.AddCommand(new DelayCommand(Constants.GlobalItemsSchedulerEvent, Constants.GlobalItemsSchedulerEventInterval), ctx =>
            {
                Execute(ctx);
            } );

            base.Execute(context);
        }
    }
}