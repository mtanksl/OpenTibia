using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IClient 
    {
        Player Player { get; set;  }

        IConnection Connection { get; set; }

        FightMode FightMode { get; set; }

        ChaseMode ChaseMode { get; set; }

        SafeMode SafeMode { get; set; }

        ICreatureCollection CreatureCollection { get; }

        IContainerCollection ContainerCollection { get; }
    }
}