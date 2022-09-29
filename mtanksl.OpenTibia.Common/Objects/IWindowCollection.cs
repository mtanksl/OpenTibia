using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IWindowCollection
    {
        byte OpenWindow(Window window);

        void ReplaceWindow(Window window, byte windowId);

        void CloseWindow(byte windowId);

        Window GetWindow(byte windowId);

        IEnumerable<Window> GetWindows();

        IEnumerable< KeyValuePair<byte, Window> > GetIndexedWindows();
    }
}