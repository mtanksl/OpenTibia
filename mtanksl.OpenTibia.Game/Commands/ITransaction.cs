namespace OpenTibia.Game.Commands
{
    public interface ITransaction
    {
        bool Execute();

        void Rollback();
    }
}