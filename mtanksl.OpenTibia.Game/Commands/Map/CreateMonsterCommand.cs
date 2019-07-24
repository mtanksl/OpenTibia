using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreateMonsterCommand : Command
    {
        public CreateMonsterCommand(string name, Position toPosition)
        {
            Name = name;

            ToPosition = toPosition;
        }

        public string Name { get; set; }

        public Position ToPosition { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Monster monster = server.MonsterFactory.Create(Name);

            if (monster != null)
            {
                Tile toTile = server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    //Act

                    server.Map.AddCreature(monster);

                    byte toIndex = toTile.AddContent(monster);

                    //Notify

                    foreach (var observer in server.Map.GetPlayers() )
                    {
                        if ( observer.Tile.Position.CanSee(ToPosition) )
                        {
                            uint removeId;

                            if (observer.Client.CreatureCollection.IsKnownCreature(monster.Id, out removeId) )
                            {
                                context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(ToPosition, toIndex, monster) );
                            }
                            else
                            {
                                context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(ToPosition, toIndex, removeId, monster) );
                            }
                        }
                    }

                    base.Execute(server, context);
                }
            }
        }
    }
}