namespace OpenTibia.Game.Scripts
{
    public interface IScript
    {
        void Start(Server server);

        void Stop(Server server);
    }
}