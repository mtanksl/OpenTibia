using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IClient 
    {
        IBattleCollection Battles { get; }

        IVipCollection Vips { get; }

        IContainerCollection Containers { get; }

        IWindowCollection Windows { get; }

        IStorageCollection Storages { get; }

        Player Player { get; set;  }

        IConnection Connection { get; set; }

        FightMode FightMode { get; set; }

        ChaseMode ChaseMode { get; set; }

        SafeMode SafeMode { get; set; }

        IContent GetContent(IContainer container, byte clientIndex);

        bool TryGetIndex(IContent content, out byte clientIndex);
    }
}