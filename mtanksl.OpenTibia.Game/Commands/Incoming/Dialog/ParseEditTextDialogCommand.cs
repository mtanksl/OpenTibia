using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseEditTextDialogCommand : Command
    {
        public ParseEditTextDialogCommand(Player player, uint windowId, string text)
        {
            Player = player;

            WindowId = windowId;

            Text = text;
        }

        public Player Player { get; set; }

        public uint WindowId { get; set; }

        public string Text { get; set; }

        public override Promise Execute()
        {
            Window window = Player.Client.Windows.GetWindow(WindowId);

            if (window != null)
            {
                Player.Client.Windows.CloseWindow(WindowId);

                if (window.Item is ReadableItem readableItem && Text != readableItem.Text)
                {
                    readableItem.Text = Text;

                    readableItem.Author = Player.Name;

                    readableItem.Date = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}