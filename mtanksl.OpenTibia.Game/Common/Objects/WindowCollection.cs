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

            return windowId;
        }

        public void OpenWindow(byte windowId, Window window)
        {
            windows[windowId] = window;
        }

        public void CloseWindow(byte windowId)
        {
            windows[windowId] = null;
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