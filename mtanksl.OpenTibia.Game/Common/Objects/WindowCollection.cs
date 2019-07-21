using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class WindowCollection : IWindowCollection
    {
        public WindowCollection(IClient client)
        {
            this.client = client;
        }

        private IClient client;

        private IClient Client
        {
            get
            {
                return client;
            }
        }

        private Window[] windows = new Window[255];

        private byte GenerateWindowId()
        {
            for (byte windowId = 0; windowId < windows.Length; windowId++)
            {
                if (windows[windowId] == null)
                {
                    return windowId;
                }
            }

            throw new Exception();
        }

        public byte OpenWindow(Window window)
        {
            byte windowId = GenerateWindowId();

            windows[windowId] = window;

            window.AddPlayer(client.Player);

            return windowId;
        }

        public void ReplaceWindow(byte windowId, Window newWindow)
        {
            Window oldWindow = GetWindow(windowId);

            oldWindow.RemovePlayer(client.Player);

            windows[windowId] = newWindow;

            newWindow.AddPlayer(client.Player);
        }

        public void CloseWindow(byte windowId)
        {
            Window window = GetWindow(windowId);

            windows[windowId] = null;

            window.RemovePlayer(client.Player);
        }

        public Window GetWindow(byte windowId)
        {
            if (windowId < 0 || windowId > windows.Length - 1)
            {
                return null;
            }

            return windows[windowId];
        }

        public IEnumerable<Window> GetWindows()
        {
            foreach (var window in windows)
            {
                if (window != null)
                {
                    yield return window;
                }
            }
        }

        public IEnumerable< KeyValuePair<byte, Window> > GetIndexedWindows()
        {
            for (byte windowId = 0; windowId < windows.Length; windowId++)
            {
                if (windows[windowId] != null)
                {
                    yield return new KeyValuePair<byte, Window>(windowId, windows[windowId] );
                }
            }
        }
    }
}