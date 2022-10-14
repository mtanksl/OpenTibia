using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class GlobalCreaturesCommand : Command
    {
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                foreach (var creature in context.Server.GameObjects.GetCreatures() )
                {
                    foreach (var component in creature.GetComponents<PeriodicBehaviour>() )
                    {
                        component.Update(context);
                    }
                }

                resolve(context);
            } );
        }
    }
}