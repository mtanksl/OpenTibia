using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCorpseCommand : CommandResult<Item>
    {
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
                    Item amulet = (Item)Player.Inventory.GetContent( (byte)Slot.Amulet);

                    if (amulet == null || amulet.Metadata.OpenTibiaId != Constants.AmuletOfLossOpenTibiaId)
                    {
                        List<Promise> promises = new List<Promise>();

                        foreach (var item in Player.Inventory.GetItems().ToList() )
                        {
                            if (item is Container || Context.Server.Randomization.HasProbability(10.0 / 100) )
                            {
                                promises.Add(Context.AddCommand(new ItemMoveCommand(item, container, 0) ) );
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