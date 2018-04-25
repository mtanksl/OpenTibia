using System;
using System.Collections.Generic;

namespace OpenTibia.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using ( Game game = new Game() )
            {
                using ( game.Log.Measure("Server online", false) )
	            {
		            game.Start();
                }

                List<Monster> monsters = new List<Monster>();

                for (int x = 0; x < 25; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        monsters.Add(RegisterMonster(new Position(940 + x, 787 + y, 7), "Scarab") );
                    }
                }

                Console.ReadKey();

                using ( game.Log.Measure("Server offline", false) )
                {
                    foreach (var monster in monsters)
	                {
                        monster.WalkSchedulerEvent.Cancel();
	                }

                    game.Stop();
                }
            }
        }

        private static Monster RegisterMonster(Position position, string name)
        {
            Monster monster = Game.Current.Map.AddCreature( Game.Current.MonsterFactory.Create(name) );

            Game.Current.Map.GetTile(position).AddContent(monster);

            Action loop = null; loop = () =>
            {
                monster.WalkSchedulerEvent = Game.Current.Scheduler.QueueForExecution(monster.WalkDelay, () =>
                {
                    foreach (var moveDirection in new MoveDirection[] { MoveDirection.North, MoveDirection.East, MoveDirection.South, MoveDirection.West }.Shuffle() )
                    {
                        if ( monster.CanMove(moveDirection) )
                        {
                            monster.Move(moveDirection);

                            break;
                        }
                    }

                    if (monster.Tile != null)
                    {
                        loop();
                    }
                } );
            };

            loop();

            return monster;
        }
    }
}