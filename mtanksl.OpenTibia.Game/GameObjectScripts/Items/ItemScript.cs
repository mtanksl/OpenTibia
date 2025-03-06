using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class ItemScript : GameObjectScript<Item>
    {
        public override void Start(Item item)
        {
            if (item.Metadata.DurationInMilliseconds != null && item.Metadata.DurationInMilliseconds > 0 && item.DurationInMilliseconds > 0)
            {
                if (item.Metadata.DecayToOpenTibiaId == null || item.Metadata.DecayToOpenTibiaId == 0)
                {
                    Context.Server.GameObjectComponents.AddComponent(item, new ItemDecayDestroyBehaviour() );
                }
                else
                {
                    Context.Server.GameObjectComponents.AddComponent(item, new ItemDecayTransformBehaviour() );
                }
            }
        }

        public override void Stop(Item item)
        {

        }
    }
}