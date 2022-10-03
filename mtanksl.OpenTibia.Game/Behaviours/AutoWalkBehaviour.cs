﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class AutoWalkBehaviour : TimeBehaviour
    {
        private Creature creature;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;
        }

        private bool running = false;

        public override void Update(Context context)
        {
            if (running)
            {
                return;
            }

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.IsInClientRange(observer.Tile.Position) )
                {
                    foreach (var direction in new[] { Direction.East, Direction.North, Direction.West, Direction.South }.Shuffle() )
                    {
                        Tile toTile = context.Server.Map.GetTile(creature.Tile.Position.Offset(direction) );

                        if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) )
                        {

                        }
                        else
                        {
                            running = true;

                            context.AddCommand(new CreatureMoveCommand(creature, toTile) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new DelayCommand(Constants.CreatureWalkSchedulerEvent(creature), 1000 * toTile.Ground.Metadata.Speed / creature.Speed) );

                            } ).Then(ctx =>
                            {
                                running = false;

                                Update(ctx);
                            } );

                            break;
                        }
                    }

                    break;
                }
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(Constants.CreatureWalkSchedulerEvent(creature) );
        }
    }
}