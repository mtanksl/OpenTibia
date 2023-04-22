using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IClient 
    {
        IBattleCollection CreatureCollection { get; }

        IVipCollection VipCollection { get; }

        IContainerCollection ContainerCollection { get; }

        IWindowCollection WindowCollection { get; }

        Player Player { get; set;  }

        IConnection Connection { get; set; }

        FightMode FightMode { get; set; }

        ChaseMode ChaseMode { get; set; }

        SafeMode SafeMode { get; set; }

        IContent GetContent(IContainer container, byte clientIndex);

        bool TryGetIndex(IContent content, out byte clientIndex);
    }
}