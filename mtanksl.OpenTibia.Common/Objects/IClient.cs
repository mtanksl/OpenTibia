using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IClient 
    {
        IBattleCollection CreatureCollection { get; }

        IContainerCollection ContainerCollection { get; }

        IWindowCollection WindowCollection { get; }

        Player Player { get; set;  }

        IConnection Connection { get; set; }

        FightMode FightMode { get; set; }

        ChaseMode ChaseMode { get; set; }

        SafeMode SafeMode { get; set; }
    }
}