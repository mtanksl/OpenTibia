using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class CombatControlsCommand : Command
    {
        private Server server;

        public CombatControlsCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public FightMode FightMode { get; set; }

        public ChaseMode ChaseMode { get; set; }

        public SafeMode SafeMode { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            Player.Client.FightMode = FightMode;

            Player.Client.ChaseMode = ChaseMode;

            Player.Client.SafeMode = SafeMode;
            
            //Notify
        }
    }
}