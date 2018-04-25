using System;

namespace OpenTibia
{
    public static class EventHandlers
    {
        public static void NotifyCreatureMove(CreatureMoveEvent e)
        {
            if (e.Creature is Player)
            {
                return;
            }

            //TODO

            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                bool canSeeFrom = observer.Tile.Position.CanSee(e.FromTile.Position);

                bool canSeeTo = observer.Tile.Position.CanSee(e.ToTile.Position);

                if (canSeeFrom && canSeeTo)
                {
                    observer.Client.Response.Write( new WalkOutgoingPacket(e.FromTile.Position, e.FromIndex, e.ToTile.Position) );
                }
                else if (canSeeFrom)
                {
                    observer.Client.Response.Write( new ThingRemoveOutgoingPacket(e.FromTile.Position, e.FromIndex) );
                }
                else if (canSeeTo)
                {
                    uint removeId;

                    if (observer.Client.IsKnownCreature(e.Creature.Id, out removeId) )
                    {
                        observer.Client.Response.Write( new ThingAddOutgoingPacket(e.ToTile.Position, e.ToIndex, e.Creature) );
                    }
                    else
                    {
                        observer.Client.Response.Write( new ThingAddOutgoingPacket(e.ToTile.Position, e.ToIndex, removeId, e.Creature) );
                    }
                }                
            }            
        }
        
        public static void WalkTile(CreatureMoveEvent e)
        {
            ushort[,] transforms = new ushort[,] { { 416, 417 }, { 417, 416 }, { 425, 426 }, { 426, 425 }, { 446, 447 }, { 447, 446 } };

            for (int i = 0; i < transforms.GetLength(0); i++)
            {
                Item before = e.FromTile.GetItem(transforms[i, 0]);

                if (before != null)
                {
                    Item after = Game.Current.ItemFactory.Create(transforms[i, 1]);

                    byte index = (byte)e.FromTile.ReplaceContent(before, after);

                    Game.Current.EventBus.Publish(new TileReplaceEvent(e.FromTile, index, before, after));

                    break;
                }
            }

            for (int i = 0; i < transforms.GetLength(0); i++)
            {
                Item before = e.ToTile.GetItem(transforms[i, 0]);

                if (before != null)
                {
                    Item after = Game.Current.ItemFactory.Create(transforms[i, 1]);

                    byte index = (byte)e.ToTile.ReplaceContent(before, after);

                    Game.Current.EventBus.Publish(new TileReplaceEvent(e.ToTile, index, before, after));

                    break;
                }
            }
        }

        public static void NotifyTileReplace(TileReplaceEvent e)
        {
            foreach (var observer in Game.Current.Map.GetPlayers())
            {
                if (observer.Tile.Position.CanSee(e.Tile.Position))
                {
                    observer.Client.Response.Write(new ThingUpdateOutgoingPacket(e.Tile.Position, e.Index, e.After));
                }
            }
        }
        
        public static void WalkFire(CreatureMoveEvent e)
        {
            if (e.Creature is Player)
            {
                return;
            }

            ushort[] transforms = new ushort[] { 1424 };

            for (int i = 0; i < transforms.Length; i++)
            {
                Item before = e.ToTile.GetItem(transforms[i]);

                if (before != null)
                {
                    Game.Current.EventBus.Publish( new MagicEffectEvent(e.Creature.Tile.Position, MagicEffectType.FirePlume) );

                    e.Creature.ChangeHealth( (ushort)Math.Max(0, e.Creature.Health - 50) );

                    break;
                }
            }
        }

        public static void NotifyMagicEffect(MagicEffectEvent e)
        {
            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.CanSee(e.Position) )
                {
                    observer.Client.Response.Write( new MagicEffectOutgoingPacket(e.Position, e.MagicEffectType) );
                }
            }
        }

        public static void NotifyCreatureChangeHealth(CreatureChangeHealthEvent e)
        {
            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.CanSee(e.Creature.Tile.Position) )
                {
                    observer.Client.Response.Write( new HealthOutgoingPacket(e.Creature.Id, (byte)Math.Ceiling(100.0 * e.ToHealth / e.Creature.MaxHealth) ) );
                }
            }
        }

        public static void NotifyCreatureRemove(CreatureRemoveEvent e)
        {
            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.CanSee(e.FromTile.Position) )
                {
                    observer.Client.Response.Write( new ThingRemoveOutgoingPacket(e.FromTile.Position, e.FromIndex) );
                }
            }
        }
        
        public static void NotifyCreatureTurn(CreatureTurnEvent e)
        {
            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.CanSee(e.Creature.Tile.Position) )
                {
                    observer.Client.Response.Write(new ThingUpdateOutgoingPacket(e.Creature.Tile.Position, (byte)e.Creature.Tile.GetIndex(e.Creature), e.Creature.Id, e.ToDirection) );
                }
            }
        }

        public static void NotifyCreatureChangeOutfit(CreatureChangeOutfitEvent e)
        {
            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.CanSee(e.Creature.Tile.Position) )
                {
                    observer.Client.Response.Write( new ChangeOutfitOutgoingPacket(e.Creature.Id, e.ToOutfit) );
                }
            }
        }
    }
}