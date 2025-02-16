using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemDecayTransformCommand : CommandResult<Item>
    {
        public ItemDecayTransformCommand(Item item, TimeSpan executeIn, ushort openTibiaId, byte typeCount)
        {
            Item = item;

            ExecuteIn = executeIn;

            OpenTibiaId = openTibiaId;

            TypeCount = typeCount;
        }

        public Item Item { get; set; }

        public TimeSpan ExecuteIn { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte TypeCount { get; set; }

        public override PromiseResult<Item> Execute()
        {
            return Context.Server.GameObjectComponents.AddComponent(Item, new ItemDecayDelayBehaviour(ExecuteIn) ).Promise.Then( () =>
            { 
                return Context.AddCommand(new ItemTransformCommand(Item, OpenTibiaId, TypeCount) );
            } );
        }
    }
}