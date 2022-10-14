using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class GlobalCreaturesCommand : Command
    {
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                foreach (var component in context.Server.Components.GetComponents<PeriodicBehaviour>().ToList() )
                {
                    if (component.GameObject is Creature)
                    {
                        component.Update(context);
                    }
                }

                resolve(context);
            } );
        }
    }
}