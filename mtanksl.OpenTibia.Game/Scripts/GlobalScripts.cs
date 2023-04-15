using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        public override void Start(Server server)
        {
            server.QueueForExecution( () =>
            {
                return Promise.WhenAll(GlobalCreatures(), GlobalLight() );
            } );
        }

        private async Promise GlobalCreatures()
        {
            while (true)
            {
                foreach (var component in Context.Server.Components.GetComponentsOfType<Creature, CreatureThinkBehaviour>().ToList() )
                {
                    if (component.GameObject != null)
                    {
                        _ = component.Update().Catch(ex =>
                        {
                            if (ex is PromiseCanceledException)
                            {
                            
                            }
                            else
                            {
                                Context.Server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                            }
                        } );
                    }
                }

                await Promise.Delay("GlobalCreatures", 100);
            }
        }

        private async Promise GlobalLight()
        {
            while (true)
            {
                Context.Server.Clock.Tick();

                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    Context.AddPacket(observer.Client.Connection, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );
                }

                await Promise.Delay("GlobalLight", Clock.Interval);
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution("GlobalCreatures");

            server.CancelQueueForExecution("GlobalLight");
        }
    }
}