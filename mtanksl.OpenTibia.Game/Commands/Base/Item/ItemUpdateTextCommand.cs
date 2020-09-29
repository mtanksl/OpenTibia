using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateTextCommand : Command
    {
        public ItemUpdateTextCommand(ReadableItem item, string text)
        {
            Item = item;

            Text = text;
        }

        public ReadableItem Item { get; set; }

        public string Text { get; set; }

        public override void Execute(Context context)
        {
            if (Item.Text != Text)
            {
                Item.Text = Text;
            }

            base.OnCompleted(context);
        }
    }
}