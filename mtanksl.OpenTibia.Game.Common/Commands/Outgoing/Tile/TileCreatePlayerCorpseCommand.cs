using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCorpseCommand : CommandResult<Item>
    {
        private readonly ushort amuletOfLoss;
        private readonly ushort humanMaleCorpse;
        private readonly ushort humanFemaleCorpse;

        public TileCreatePlayerCorpseCommand(Tile tile, Player player, bool dropAll, int blesses)
        {
            amuletOfLoss = Context.Server.Values.GetUInt16("values.items.amuletOfLoss");
            humanMaleCorpse = Context.Server.Values.GetUInt16("values.items.humanMaleCorpse");
            humanFemaleCorpse = Context.Server.Values.GetUInt16("values.items.humanFemaleCorpse");

            Tile = tile;

            Player = player;

            DropAll = dropAll;

            Blesses = blesses;
        }

        public Tile Tile { get; set; }

        public Player Player { get; set; }

        public bool DropAll { get; set; }

        public int Blesses { get; set; }

        public override PromiseResult<Item> Execute()
        {
            return Context.AddCommand(new TileCreateItemCommand(Tile, Player.Gender == Gender.Male ? humanMaleCorpse : humanFemaleCorpse, 1) ).Then( (corpse) =>
            {
                if (corpse is Container container)
                {
                    List<Promise> promises = new List<Promise>();

                    if (DropAll)
                    {
                        Item amulet = (Item)Player.Inventory.GetContent( (byte)Slot.Amulet);

                        if (amulet != null && amulet.Metadata.OpenTibiaId == amuletOfLoss)
                        {
                            promises.Add(Context.AddCommand(new ItemDestroyCommand(amulet) ) );
                        }

                        foreach (var item in Player.Inventory.GetItems().ToArray() )
                        {
                            if (item != amulet)
                            {
                                promises.Add(Context.AddCommand(new ItemMoveCommand(item, container, 0) ) );
                            }
                        }
                    }
                    else
                    {
                        Item amulet = (Item)Player.Inventory.GetContent( (byte)Slot.Amulet);

                        if (amulet != null && amulet.Metadata.OpenTibiaId == amuletOfLoss)
                        {
                            promises.Add(Context.AddCommand(new ItemDestroyCommand(amulet) ) );
                        }
                        else
                        {
                            double containerLoss = Formula.GetContainerLossPercent(Blesses);

                            double equipmentLoss = Formula.GetEquipmentLossPercent(Blesses);

                            foreach (var item in Player.Inventory.GetItems().ToArray() )
                            {
                                if (item is Container)
                                {
                                    if (Context.Server.Randomization.HasProbability(containerLoss) )
                                    {
                                        promises.Add(Context.AddCommand(new ItemMoveCommand(item, container, 0) ) );
                                    }
                                }
                                else
                                {
                                    if (Context.Server.Randomization.HasProbability(equipmentLoss) )
                                    {
                                        promises.Add(Context.AddCommand(new ItemMoveCommand(item, container, 0) ) );
                                    }
                                }
                            }
                        }
                    }

                    return Promise.WhenAll(promises.ToArray() ).Then( () =>
                    {
                        return Promise.FromResult(corpse);
                    } );
                }

                return Promise.FromResult(corpse);
            } );
        }
    }
}