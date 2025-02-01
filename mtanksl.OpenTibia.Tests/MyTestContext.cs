using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Scripts;

namespace OpenTibia.Tests
{
    [TestClass]
    public class MyTestContext
    {
        private static IServer server;

        public static IServer Server
        {
            get
            {
                return server;
            }
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            server = new Server();

                server.Logger = new Logger(new DebugLoggerProvider(), LogLevel.Debug);

                server.MessageCollectionFactory = new MockMessageCollectionFactory();

            server.Start();

            server.QueueForExecution( () =>
            {
                server.Scripts.GetScript<GlobalScripts>().Stop();

                return Promise.Completed;
            } );
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            server.Stop();

            server.Dispose();

            server = null;
        }
    }
}