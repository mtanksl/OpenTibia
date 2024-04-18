using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemCloneCommand : CommandResult<Item>
    {
        public ItemCloneCommand(Item fromItem, bool deepClone)
        {
            FromItem = fromItem;

            DeepClone = deepClone;
        }

        public Item FromItem { get; set; }

        public bool DeepClone { get; set; }

        public override async PromiseResult<Item> Execute()
        {
            byte count = 1;

            if (FromItem is StackableItem stackableItem)
            {
                count = stackableItem.Count;
            }
            else if (FromItem is FluidItem fluidItem)
            {
                count = (byte)fluidItem.FluidType;
            }
            else if (FromItem is SplashItem splashItem)
            {
                count = (byte)splashItem.FluidType;
            }

            Item toItem = Context.Server.ItemFactory.Create(FromItem.Metadata.OpenTibiaId, count);

            if (toItem != null)
            {
                Context.Server.ItemFactory.Attach(toItem);

                toItem.ActionId = FromItem.ActionId;

                // toItem.UniqueId = FromItem.UniqueId;

                if (FromItem is Container fromContainer)
                {
                    if (toItem is Container toContainer)
                    {
                        if (DeepClone)
                        {
                            foreach (var item in fromContainer.GetItems() )
                            {
                                Item clone = await Context.AddCommand(new ItemCloneCommand(item, true) );

                                toContainer.AddContent(clone);
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("ToItem must be Container.");
                    }
                }
                else if (FromItem is DoorItem fromDoorItem)
                {
                    if (toItem is DoorItem toDoorItem)
                    {
                        toDoorItem.DoorId = fromDoorItem.DoorId;
                    }
                    else 
                    {
                        throw new InvalidOperationException("ToItem must be DoorItem."); 
                    }                    
                }
                else if (FromItem is ReadableItem fromReadableItem)
                {
                    if (toItem is ReadableItem toReadableItem)
                    {
                        toReadableItem.Text = fromReadableItem.Text;

                        toReadableItem.Author = fromReadableItem.Author;

                        toReadableItem.Date = fromReadableItem.Date;
                    }
                    else 
                    {
                        throw new InvalidOperationException("ToItem must be ReadableItem."); 
                    }                    
                }
                else if (FromItem is TeleportItem fromTeleportItem)
                {
                    if (toItem is TeleportItem toTeleportItem)
                    {
                        toTeleportItem.Position = fromTeleportItem.Position;
                    }
                    else 
                    {
                        throw new InvalidOperationException("ToItem must be TeleportItem."); 
                    }                    
                }
            }

            return toItem;
        }
    }
}
