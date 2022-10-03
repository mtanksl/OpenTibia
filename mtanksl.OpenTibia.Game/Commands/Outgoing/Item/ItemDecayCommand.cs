using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ItemDecayCommand : Command
    {
        public ItemDecayCommand(Item item, int executeInMilliseconds, ushort openTibiaId, byte count)
        {
            Item = item;

            ExecuteInMilliseconds = executeInMilliseconds;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Item Item { get; set; }

        public int ExecuteInMilliseconds { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            var component = Item.GetComponent<ItemDecayBehaviour>();

            if (component != null)
            {
                Item.RemoveComponent(component);
            }

            Item.AddComponent(new ItemDecayBehaviour(ExecuteInMilliseconds, OpenTibiaId, Count) );

            OnComplete(context);
        }
    }
}