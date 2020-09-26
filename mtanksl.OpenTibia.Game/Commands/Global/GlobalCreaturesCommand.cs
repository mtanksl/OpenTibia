using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class GlobalCreaturesCommand : Command
    {
        public override void Execute(Context context)
        {
            foreach (var creature in context.Server.GameObjects.GetMonsterAndNpcs() )
            {
                foreach (var component in creature.GetComponents<TimeBehaviour>() )
                {
                    component.Update(context);
                }
            }

            base.OnCompleted(context);
        }
    }
}