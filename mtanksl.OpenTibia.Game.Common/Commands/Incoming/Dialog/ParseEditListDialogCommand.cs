using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseEditListDialogCommand : IncomingCommand
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

                if (window.House != null)
                {
                    if (window.DoorId == 0xFE)
                    {
                        HouseAccessList houseAccessList = window.House.GetSubOwnersList();

                        houseAccessList.SetText(Text);
                    }
                    else if (window.DoorId == 0xFF)
                    {
                        HouseAccessList houseAccessList = window.House.GetGuestsList();

                        houseAccessList.SetText(Text);
                    }
                    else
                    {
                        HouseAccessList houseAccessList = window.House.GetDoorList(window.DoorId);

                        houseAccessList.SetText(Text);
                    }
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}