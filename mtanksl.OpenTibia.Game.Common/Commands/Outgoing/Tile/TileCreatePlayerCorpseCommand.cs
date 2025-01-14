using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCorpseCommand : CommandResult<Item>
    {
        private static readonly double[] equipmentLosses = new double[] { 10, 7, 4.5, 2.5, 1, 0 };

        private static readonly double[] containerLosses = new double[] { 100, 70, 45, 25, 10, 0 };

        public TileCreatePlayerCorpseCommand(Tile tile, Player player)
        {
            Tile = tile;

            Player = player;
        }

        public Tile Tile { get; set; }

        public Player Player { get; set; }

        public override PromiseResult<Item> Execute()
        {
            return Context.AddCommand(new TileCreateItemCommand(Tile, Player.Gender == Gender.Male ? Constants.HumanMaleCorpseOpenTibiaItemId : Constants.HumanFemaleCorpseOpenTibiaItemId, 1) ).Then( (corpse) =>
            {
                if (corpse is Container container)
                {
                    int blesses = Player.Blesses.Count;

                    Player.Blesses.ClearBlesses();

                    double equipmentLoss = equipmentLosses[Math.Min(blesses, equipmentLosses.Length - 1) ];

                    double containerLoss = containerLosses[Math.Min(blesses, containerLosses.Length - 1) ];

                    Item amulet = (Item)Player.Inventory.GetContent( (byte)Slot.Amulet);

                    if (amulet == null || amulet.Metadata.OpenTibiaId != Constants.AmuletOfLossOpenTibiaId)
                    {
                        List<Promise> promises = new List<Promise>();

                        foreach (var item in Player.Inventory.GetItems().ToArray() )
                        {
                            if (item is Container)
                            {
                                if (Context.Server.Randomization.HasProbability(containerLoss / 100) )
                                {
                                    promises.Add(Context.AddCommand(new ItemMoveCommand(item, container, 0) ) );
                                }
                            }
                            else
                            {
                                if (Context.Server.Randomization.HasProbability(equipmentLoss / 100) )
                                {
                                    promises.Add(Context.AddCommand(new ItemMoveCommand(item, container, 0) ) );
                                }
                            }
                        }

                        return Promise.WhenAll(promises.ToArray() ).Then( () =>
                        {
                            return Promise.FromResult(corpse);
                        } );
                    }
                    else
                    {
                        return Context.AddCommand(new ItemDestroyCommand(amulet) ).Then( () =>
                        {
                            return Promise.FromResult(corpse);
                        } );
                    }
                }

                return Promise.FromResult(corpse);
            } );
        }
    }
}