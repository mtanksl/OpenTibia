namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IWaitingList
    {
        bool CanLogin(string name, out int position, out byte time);
    }
}