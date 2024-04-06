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

                if (window.House != null && window.DoorId == DoorId)
                {
                    if (DoorId == 0x01)
                    {
                        HouseAccessList houseAccessList = window.House.GetSubOwnersList();

                        houseAccessList.SetText(Text);
                    }
                    else if (DoorId == 0x02)
                    {
                        HouseAccessList houseAccessList = window.House.GetGuestsList();

                        houseAccessList.SetText(Text);
                    }
                    else if (DoorId == 0x03)
                    {
                        HouseAccessList houseAccessList = window.House.GetDoorList(window.DoorPosition);

                        houseAccessList.SetText(Text);
                    }
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}