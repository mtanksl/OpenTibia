using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class GlobalCreaturesCommand : Command
    {
        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                foreach (var component in Context.Server.Components.GetComponentsOfType<Creature, PeriodicBehaviour>().ToList() )
                {
                    if (component.GameObject != null)
                    {
                        component.Update();
                    }
                }

                resolve();
            } );
        }
    }
}