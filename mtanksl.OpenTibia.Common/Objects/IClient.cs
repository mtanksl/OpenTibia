using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IClient 
    {
        Dictionary<string, object> Data { get; }

        string AccountNumber { get; set; }

        AccountManagerType AccountManagerType { get; set; }

        IBattleCollection Battles { get; }

        IContainerCollection Containers { get; }

        IWindowCollection Windows { get; }

        Player Player { get; set;  }

        IConnection Connection { get; set; }

        FightMode FightMode { get; set; }

        ChaseMode ChaseMode { get; set; }

        SafeMode SafeMode { get; set; }

        IContent GetContent(IContainer container, byte clientIndex);

        bool TryGetIndex(IContent content, out byte clientIndex);
    }
}