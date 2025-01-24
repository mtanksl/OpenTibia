namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IRaidCollection
    {
        bool Start(string name);

        void Start();

        void Stop();
    }
}