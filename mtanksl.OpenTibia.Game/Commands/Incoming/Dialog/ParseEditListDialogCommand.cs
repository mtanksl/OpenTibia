using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseEditListDialogCommand : Command
    {
        public ParseEditListDialogCommand(Player player, byte doorId, uint windowId, string text)
        {
            Player = player;

            DoorId = doorId;

            WindowId = windowId;

            Text = text;
        }

        public Player Player { get; set; }

        public byte DoorId { get; set; }

        public uint WindowId { get; set; }

        public string Text { get; set; }

        public override Promise Execute()
        {
            Window window = Player.Client.Windows.GetWindow(WindowId);

            if (window != null)
            {
                Player.Client.Windows.CloseWindow(WindowId);

                

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}