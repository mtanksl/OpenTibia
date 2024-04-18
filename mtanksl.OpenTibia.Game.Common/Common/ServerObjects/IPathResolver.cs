namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPathResolver
    {
        bool Exists(string relativePath);

        string GetFullPath(string relativePath);
    }
}