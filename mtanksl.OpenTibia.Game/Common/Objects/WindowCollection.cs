using OpenTibia.Common.Objects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Common
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
     
        /// <exception cref="InvalidOperationException"></exception>

        private uint GenerateWindowId()
        {
            for (uint windowId = 0; windowId < windows.Length; windowId++)
            {
                if (windows[windowId] == null)
                {
                    return windowId;
                }
            }

            throw new InvalidOperationException("Window limit exceeded.");
        }

        public uint OpenWindow(Window window)
        {
            uint windowId = GenerateWindowId();

            windows[windowId] = window;

            window.AddPlayer(client.Player);

            return windowId;
        }

        public void ReplaceWindow(Window newWindow, uint windowId)
        {
            Window oldWindow = GetWindow(windowId);

            oldWindow.RemovePlayer(client.Player);

            windows[windowId] = newWindow;

            newWindow.AddPlayer(client.Player);
        }

        public void CloseWindow(uint windowId)
        {
            Window window = GetWindow(windowId);

            windows[windowId] = null;

            window.RemovePlayer(client.Player);
        }

        public Window GetWindow(uint windowId)
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

        public IEnumerable< KeyValuePair<uint, Window> > GetIndexedWindows()
        {
            for (uint windowId = 0; windowId < windows.Length; windowId++)
            {
                if (windows[windowId] != null)
                {
                    yield return new KeyValuePair<uint, Window>(windowId, windows[windowId] );
                }
            }
        }
    }
}