using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IWindowCollection
    {
        uint OpenWindow(Window window);

        void ReplaceWindow(Window window, uint windowId);

        void CloseWindow(uint windowId);

        Window GetWindow(uint windowId);

        IEnumerable<Window> GetWindows();

        IEnumerable< KeyValuePair<uint, Window> > GetIndexedWindows();
    }
}