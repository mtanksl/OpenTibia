namespace OpenTibia.Common.Objects
{
    public interface IWindowCollection
    {
        Window GetWindow(byte windowId);

        bool HasWindow(Window window, out byte windowId);

        byte OpenWindow(Window window);

        byte CloseWindow(Window window);
    }
}