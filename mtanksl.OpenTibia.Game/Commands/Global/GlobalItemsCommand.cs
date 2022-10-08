using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class GlobalItemsCommand : Command
    {
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                foreach (var item in context.Server.GameObjects.GetItems() )
                {
                    foreach (var component in item.GetComponents<TimeBehaviour>() )
                    {
                        component.Update(context);
                    }
                }

                resolve(context);
            } );
        }
    }
}