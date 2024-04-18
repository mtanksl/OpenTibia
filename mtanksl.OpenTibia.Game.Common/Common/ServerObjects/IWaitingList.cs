namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IWaitingList
    {
        bool CanLogin(int databasePlayerId, out int position, out byte time);
    }
}