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

        private Dictionary<byte, Window> windows = new Dictionary<byte, Window>();

        private Dictionary<Window, byte> windowIds = new Dictionary<Window, byte>();

        private byte GenerateId()
        {
            for (byte id = 0; id < 255; id++)
            {
                if ( !windows.ContainsKey(id) )
                {
                    return id;
                }
            }

            throw new Exception();
        }

        public Window GetWindow(byte windowId)
        {
            Window window;

            windows.TryGetValue(windowId, out window);

            return window;
        }

        public bool HasWindow(Window window, out byte windowId)
        {
            return windowIds.TryGetValue(window, out windowId);
        }

        public byte OpenWindow(Window window)
        {
            byte windowId = GenerateId();

            windows.Add(windowId, window);

            windowIds.Add(window, windowId);

            return windowId;
        }

        public byte CloseWindow(Window window)
        {
            byte windowId;

            if ( HasWindow(window, out windowId) )
            {
                windows.Remove(windowId);

                windowIds.Remove(window);
            }

            return windowId;
        }
    }
}