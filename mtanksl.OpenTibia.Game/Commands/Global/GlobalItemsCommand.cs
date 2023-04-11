using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class GlobalItemsCommand : Command
    {
        public override Promise Execute()
        {
            foreach (var component in Context.Server.Components.GetComponentsOfType<Item, PeriodicBehaviour>().ToList() )
            {
                if (component.GameObject != null)
                {
                    component.Update();
                }
            }

            return Promise.Completed;
        }
    }
}