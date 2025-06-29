using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.Game.GameObjectScripts;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;
using System.Linq;
using Item = OpenTibia.Common.Objects.Item;
using ItemFlags = OpenTibia.FileFormats.Dat.ItemFlags;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class ItemFactory : IItemFactory
    {
        private HashSet<ushort> magicForcefields;
        private HashSet<ushort> lockers;
        private HashSet<ushort> doors;

        private IServer server;

        public ItemFactory(IServer server)
        {
            this.server = server;
        }

        public void Start(OtbFile otbFile, DatFile datFile, ItemsFile itemsFile)
        {
            magicForcefields = server.Values.GetUInt16HashSet("values.items.magicForcefields");

            lockers = server.Values.GetUInt16HashSet("values.items.lockers");

            doors = new HashSet<ushort>();

            foreach (var item in server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.openDoors") )
            {
                doors.Add(item.Key);
            }

            foreach (var item in server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.closeHorizontalDoors") )
            {
                doors.Add(item.Key);
            }

            foreach (var item in server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.closeVerticalDoors") )
            {
                doors.Add(item.Key);
            }

            foreach (var item in server.Values.GetUInt16HashSet("values.items.lockedDoors") )
            {
                doors.Add(item);
            }

            openTibiaMetadatas = new Dictionary<ushort, ItemMetadata>(datFile.Items.Count);

            tibiaMetadatas = new Dictionary<ushort, List<ItemMetadata> >(datFile.Items.Count);

            foreach (var otbItem in otbFile.Items)
            {
                if (otbItem.Group != ItemGroup.Deprecated)
                {
                    ItemMetadata metadata = new ItemMetadata()
                    {
                        TibiaId = otbItem.TibiaId,

                        OpenTibiaId = otbItem.OpenTibiaId,

                        SkillModifier = new Dictionary<Skill, int>(),

                        DamageTakenFromElements = new Dictionary<DamageType, double>()
                    };

                    if (otbItem.Flags.Is(FileFormats.Otb.ItemFlags.AllowDistanceRead) )
                    {
                        metadata.Flags |= ItemMetadataFlags.AllowDistanceRead;
                    }

                    openTibiaMetadatas.Add(otbItem.OpenTibiaId, metadata);

                    List<ItemMetadata> metadatas;

                    if ( !tibiaMetadatas.TryGetValue(otbItem.TibiaId, out metadatas) )
                    {
                        metadatas = new List<ItemMetadata>();

                        tibiaMetadatas.Add(otbItem.TibiaId, metadatas);                        
                    }

                    metadatas.Add(metadata);
                }
            }

            foreach (var datItem in datFile.Items)
            {
                foreach (var metadata in tibiaMetadatas[datItem.TibiaId] )
                {
                    if (datItem.Flags.Is(ItemFlags.IsGround) )
                    {
                        metadata.TopOrder = TopOrder.Ground;
                    }
                    else if (datItem.Flags.Is(ItemFlags.AlwaysOnTop1) )
                    {
                        metadata.TopOrder = TopOrder.HighPriority;
                    }
                    else if (datItem.Flags.Is(ItemFlags.AlwaysOnTop2) )
                    {
                        metadata.TopOrder = TopOrder.MediumPriority;
                    }
                    else if (datItem.Flags.Is(ItemFlags.AlwaysOnTop3) )
                    {
                        metadata.TopOrder = TopOrder.LowPriority;
                    }
                    else
                    {
                        metadata.TopOrder = TopOrder.Other;
                    }

                    if (datItem.Flags.Is(ItemFlags.IsContainer) )
                    {
                        metadata.Flags |= ItemMetadataFlags.IsContainer;
                    }

                    if (datItem.Flags.Is(ItemFlags.Stackable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Stackable;
                    }

                    if (datItem.Flags.Is(ItemFlags.Useable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Useable;
                    }

                    if (datItem.Flags.Is(ItemFlags.Writeable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Writeable;
                    }
                      
                    if (datItem.Flags.Is(ItemFlags.Readable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Readable;
                    }

                    if (datItem.Flags.Is(ItemFlags.IsFluid) )
                    {
                        metadata.Flags |= ItemMetadataFlags.IsFluid;
                    }

                    if (datItem.Flags.Is(ItemFlags.IsSplash) )
                    {
                        metadata.Flags |= ItemMetadataFlags.IsSplash;
                    }

                    if (datItem.Flags.Is(ItemFlags.NotWalkable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.NotWalkable;
                    }

                    if (datItem.Flags.Is(ItemFlags.NotMoveable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.NotMoveable;
                    }

                    if (datItem.Flags.Is(ItemFlags.BlockProjectile) )
                    {
                        metadata.Flags |= ItemMetadataFlags.BlockProjectile;
                    }

                    if (datItem.Flags.Is(ItemFlags.BlockPathFinding) )
                    {
                        metadata.Flags |= ItemMetadataFlags.BlockPathFinding;
                    }

                    if (datItem.Flags.Is(ItemFlags.Pickupable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Pickupable;
                    }

                    if (datItem.Flags.Is(ItemFlags.Hangable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Hangable;
                    }

                    if (datItem.Flags.Is(ItemFlags.Horizontal) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Horizontal;
                    }

                    if (datItem.Flags.Is(ItemFlags.Vertical) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Vertical;
                    }

                    if (datItem.Flags.Is(ItemFlags.Rotatable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Rotatable;
                    }

                    if (datItem.ItemHeight > 0)
                    {
                        metadata.Flags |= ItemMetadataFlags.HasHeight;
                    }

                    if (datItem.Animations > 1)
                    {
                        metadata.Flags |= ItemMetadataFlags.IsAnimated;
                    }

                    if (datItem.Flags.Is(ItemFlags.Wrappable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Wrappable;
                    }

                    if (datItem.Flags.Is(ItemFlags.Unwrappable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Unwrappable;
                    }

                    metadata.GroundSpeed = datItem.Speed;

                    metadata.MaxWriteChars = datItem.MaxWriteChars;

                    metadata.MaxReadChars = datItem.MaxReadChars;

                    if (datItem.LightLevel > 0 || datItem.LightColor > 0)
                    {
                        metadata.Light = new Light( (byte)datItem.LightLevel, (byte)datItem.LightColor);
                    }
                }
            }

            foreach (var xmlItem in itemsFile.Items)
            {
                ItemMetadata metadata;

                if (openTibiaMetadatas.TryGetValue(xmlItem.OpenTibiaId, out metadata) )
                {
                    metadata.Article = xmlItem.Article;

                    metadata.Name = xmlItem.Name;

                    metadata.Plural = xmlItem.Plural;

                    metadata.Description = xmlItem.Description;

                    metadata.RuneSpellName = xmlItem.RuneSpellName;

                    metadata.Weight = xmlItem.Weight;

                    metadata.Armor = xmlItem.Armor;

                    metadata.Defense = xmlItem.Defense;

                    metadata.DefenseModifier = xmlItem.DefenseModifier;

                    metadata.Attack = xmlItem.Attack;

                    if (xmlItem.BlockProjectile == true)
                    {
                        metadata.Flags |= ItemMetadataFlags.BlockProjectile;
                    }

                    metadata.FloorChange = xmlItem.FloorChange;

                    metadata.Race = xmlItem.Race;

                    metadata.Capacity = xmlItem.ContainerSize;

                    if (metadata.Capacity == null)
                    {
                        if (metadata.Flags.Is(ItemMetadataFlags.IsContainer) )
                        {
                            metadata.Capacity = 8;
                        }
                    }
                    else
                    {
                        if ( !metadata.Flags.Is(ItemMetadataFlags.IsContainer) )
                        {
                            metadata.Flags |= ItemMetadataFlags.IsContainer;
                        }
                    }

                    metadata.WeaponType = xmlItem.WeaponType;

                    metadata.AmmoType = xmlItem.AmmoType;

                    metadata.ProjectileType = xmlItem.ProjectileType;

                    metadata.MagicEffectType = xmlItem.MagicEffectType;

                    metadata.Range = xmlItem.Range;

                    metadata.Charges = xmlItem.Charges;

                    metadata.ShowCharges = xmlItem.ShowCharges == true ? true : false;

                    metadata.DurationInMilliseconds = xmlItem.DurationInSeconds * 1000;

                    metadata.ShowDuration = xmlItem.ShowDuration == true ? true : false;

                    metadata.DecayToOpenTibiaId = xmlItem.DecayToOpenTibiaId;

                    metadata.SlotType = xmlItem.SlotType;

                    metadata.BreakChance = xmlItem.BreakChance;

                    metadata.AmmoAction = xmlItem.AmmoAction;

                    metadata.HitChance = xmlItem.HitChance;

                    metadata.MaxHitChance = xmlItem.MaxHitChance;

                    if (metadata.WeaponType == WeaponType.Distance && metadata.SlotType != SlotType.Ammo && metadata.MaxHitChance == null)
                    {
                        if (metadata.SlotType == SlotType.TwoHanded)
                        {
                            metadata.MaxHitChance = 90;
                        }
                        else
                        {
                            metadata.MaxHitChance = 75;
                        }
                    }

                    if (xmlItem.Readable == true)
                    {
                        metadata.Flags |= ItemMetadataFlags.Readable;
                    }

                    if (xmlItem.Writeable == true)
                    {
                        metadata.Flags |= ItemMetadataFlags.Writeable;
                    }
                                      
                    if (xmlItem.AbsorbPhysicalPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Physical] = (100 - xmlItem.AbsorbPhysicalPercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbEarthPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Earth] = (100 - xmlItem.AbsorbEarthPercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbFirePercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Fire] = (100 - xmlItem.AbsorbFirePercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbEnergyPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Energy] = (100 - xmlItem.AbsorbEnergyPercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbIcePercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Ice] = (100 - xmlItem.AbsorbIcePercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbDeathPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Death] = (100 - xmlItem.AbsorbDeathPercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbHolyPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Holy] = (100 - xmlItem.AbsorbHolyPercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbDrownPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.Drown] = (100 - xmlItem.AbsorbDrownPercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbManaDrainPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.ManaDrain] = (100 - xmlItem.AbsorbManaDrainPercent.Value) / 100.0;
                    }
                    
                    if (xmlItem.AbsorbLifeDrainPercent != null)
                    {
                        metadata.DamageTakenFromElements[DamageType.LifeDrain] = (100 - xmlItem.AbsorbLifeDrainPercent.Value) / 100.0;
                    }

                    if (xmlItem.AttackModifierEarth != null)
                    {
                        metadata.AttackDamageType = DamageType.Earth;

                        metadata.AttackModifier = xmlItem.AttackModifierEarth.Value;
                    }
                    else if (xmlItem.AttackModifierFire != null)
                    {
                        metadata.AttackDamageType = DamageType.Fire;

                        metadata.AttackModifier = xmlItem.AttackModifierFire.Value;
                    }
                    else if (xmlItem.AttackModifierEnergy != null)
                    {
                        metadata.AttackDamageType = DamageType.Energy;

                        metadata.AttackModifier = xmlItem.AttackModifierEnergy.Value;
                    }
                    else if (xmlItem.AttackModifierIce != null)
                    {
                        metadata.AttackDamageType = DamageType.Ice;

                        metadata.AttackModifier = xmlItem.AttackModifierIce.Value;
                    }
                    else if (xmlItem.AttackModifierDeath != null)
                    {
                        metadata.AttackDamageType = DamageType.Death;

                        metadata.AttackModifier = xmlItem.AttackModifierDeath.Value;
                    }
                    else if (xmlItem.AttackModifierHoly != null)
                    {
                        metadata.AttackDamageType = DamageType.Holy;
                            
                        metadata.AttackModifier = xmlItem.AttackModifierHoly.Value;
                    }
                    else if (xmlItem.AttackModifierDrown != null)
                    {
                        metadata.AttackDamageType = DamageType.Drown;

                        metadata.AttackModifier  = xmlItem.AttackModifierDrown.Value;
                    }
                    else if (xmlItem.AttackModifierManaDrain != null)
                    {
                        metadata.AttackDamageType = DamageType.ManaDrain;

                        metadata.AttackModifier = xmlItem.AttackModifierManaDrain.Value;
                    }
                    else if (xmlItem.AttackModifierLifeDrain != null)
                    {
                        metadata.AttackDamageType = DamageType.LifeDrain;
                                                                         
                        metadata.AttackModifier = xmlItem.AttackModifierLifeDrain.Value;
                    }

                    metadata.SpeedModifier = xmlItem.SpeedModifier;

                    if (xmlItem.SkillModifierMagicLevel != null)
                    {
                        metadata.SkillModifier[Skill.MagicLevel] = xmlItem.SkillModifierMagicLevel.Value;
                    }
                    
                    if (xmlItem.SkillModifierFist != null)
                    {
                        metadata.SkillModifier[Skill.Fist] = xmlItem.SkillModifierFist.Value;
                    }
                    
                    if (xmlItem.SkillModifierClub != null)
                    {
                        metadata.SkillModifier[Skill.Club] = xmlItem.SkillModifierClub.Value;
                    }
                    
                    if (xmlItem.SkillModifierSword != null)
                    {
                        metadata.SkillModifier[Skill.Sword] = xmlItem.SkillModifierSword.Value;
                    }
                    
                    if (xmlItem.SkillModifierAxe != null)
                    {
                        metadata.SkillModifier[Skill.Axe] = xmlItem.SkillModifierAxe.Value;
                    }
                    
                    if (xmlItem.SkillModifierDistance != null)
                    {
                        metadata.SkillModifier[Skill.Distance] = xmlItem.SkillModifierDistance.Value;
                    }
                    
                    if (xmlItem.SkillModifierShield != null)
                    {
                        metadata.SkillModifier[Skill.Shield] = xmlItem.SkillModifierShield.Value;
                    }
                    
                    if (xmlItem.SkillModifierFish != null)
                    {
                        metadata.SkillModifier[Skill.Fish] = xmlItem.SkillModifierFish.Value;
                    }
                }
            }
        }

        private Dictionary<ushort, ItemMetadata> openTibiaMetadatas;

        public ItemMetadata GetItemMetadataByOpenTibiaId(ushort openTibiaId)
        {
            ItemMetadata metadata;

            if (openTibiaMetadatas.TryGetValue(openTibiaId, out metadata) )
            {
                return metadata;
            }

            return null;
        }

        private Dictionary<ushort, List<ItemMetadata> > tibiaMetadatas;

        public ItemMetadata GetItemMetadataByTibiaId(ushort tibiaId)
        {
            List<ItemMetadata> metadatas;

            if (tibiaMetadatas.TryGetValue(tibiaId, out metadatas) )
            {
                return metadatas.FirstOrDefault();
            }

            return null;
        }

        public Item Create(ushort openTibiaId, byte typeCount)
        {
            ItemMetadata metadata = GetItemMetadataByOpenTibiaId(openTibiaId);

            if (metadata == null)
            {
                return null;
            }

            Item item;

            if (magicForcefields.Contains(openTibiaId) )
            {
                item = new TeleportItem(metadata);
            }
            else if (lockers.Contains(openTibiaId) )
            {
                item = new Locker(metadata);
            }
            else if (doors.Contains(openTibiaId) )
            {
                item = new DoorItem(metadata);
            }
            else if (metadata.Flags.Is(ItemMetadataFlags.IsContainer) )
            {
                item = new Container(metadata);
            }
            else if (metadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                item = new StackableItem(metadata)
                {
                    Count = typeCount
                };
            }
            else if (metadata.Flags.Is(ItemMetadataFlags.IsFluid) )
            {
                item = new FluidItem(metadata)
                {
                    FluidType = (FluidType)typeCount
                };
            }
            else if (metadata.Flags.Is(ItemMetadataFlags.IsSplash) )
            {
                item = new SplashItem(metadata)
                {
                    FluidType = (FluidType)typeCount
                };
            }
            else if (metadata.Flags.Is(ItemMetadataFlags.Writeable) || metadata.Flags.Is(ItemMetadataFlags.Readable) || metadata.Flags.Is(ItemMetadataFlags.AllowDistanceRead) )
            {
                item = new ReadableItem(metadata);
            }
            else
            {
                item = new Item(metadata)
                {
                    Charges = metadata.Charges == null ? 0 : metadata.Charges.Value,

                    DurationInMilliseconds = metadata.DurationInMilliseconds == null ? 0 : metadata.DurationInMilliseconds.Value
                };
            }

            item.IsDestroyed = true;

            return item;
        }

        public void Attach(Item item)
        {
            if (item.IsDestroyed)
            {
                if (item is Container container)
                {
                    foreach (var child in container.GetItems() )
                    {
                        Attach(child);
                    }
                }

                item.IsDestroyed = false;

                server.GameObjects.AddGameObject(item);

                GameObjectScript<Item> gameObjectScript = server.GameObjectScripts.GetItemGameObjectScript(item.Metadata.OpenTibiaId) ?? server.GameObjectScripts.GetItemGameObjectScript(0);

                if (gameObjectScript != null)
                {
                    gameObjectScript.Start(item);
                }

                ItemCreationPlugin plugin = server.Plugins.GetItemCreationPlugin(item.Metadata.OpenTibiaId) ?? server.Plugins.GetItemCreationPlugin(0);

                if (plugin != null)
                {
                    if (plugin.OnStart(item).Result)
                    {
                        //
                    }
                }
            }
        }

        public bool Detach(Item item)
        {
            if (server.GameObjects.RemoveGameObject(item) )
            {
                if (item is Container container)
                {
                    foreach (var child in container.GetItems() )
                    {
                        Detach(child);
                    }
                }

                GameObjectScript<Item> gameObjectScript = server.GameObjectScripts.GetItemGameObjectScript(item.Metadata.OpenTibiaId) ?? server.GameObjectScripts.GetItemGameObjectScript(0);

                if (gameObjectScript != null)
                {
                    gameObjectScript.Stop(item);
                }

                ItemCreationPlugin plugin = server.Plugins.GetItemCreationPlugin(item.Metadata.OpenTibiaId) ?? server.Plugins.GetItemCreationPlugin(0);

                if (plugin != null)
                {
                    if (plugin.OnStop(item).Result)
                    {
                        //
                    }
                }

                return true;
            }

            return false;
        }

        public void ClearComponentsAndEventHandlers(Item item)
        {
            if (item is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    ClearComponentsAndEventHandlers(child);
		        }
	        }

            server.GameObjectComponents.ClearComponents(item);

            server.GameObjectEventHandlers.ClearEventHandlers(item);

            server.PositionalEventHandlers.ClearEventHandlers(item);
        }
    }
}