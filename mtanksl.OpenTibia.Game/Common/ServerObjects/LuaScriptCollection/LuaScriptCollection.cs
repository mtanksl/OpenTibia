using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
#if AOT
using System.Diagnostics.CodeAnalysis;
#endif
using System.IO;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
#if AOT
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
#endif
    public class LuaScriptCollection : ILuaScriptCollection
    {
        private LuaScope lua;

        public LuaScriptCollection(IServer server)
        {
            lua = new LuaScope(server);

            lua.RegisterFunction("print", this, GetType().GetMethod(nameof(Print) ) );

            lua.RegisterFunction("typeof", this, GetType().GetMethod(nameof(TypeOf) ) );

            lua.RegisterFunction("cast", this, GetType().GetMethod(nameof(Cast) ) );

            lua.RegisterFunction("getconfig", this, GetType().GetMethod(nameof(GetConfig) ) );

            lua.RegisterFunction("getfullpath", this, GetType().GetMethod(nameof(GetFullPath) ) );

            lua.RegisterCoFunction("delay", parameters =>
            {                   
                string key = Guid.NewGuid().ToString();

                Promise promise = Promise.Delay(key, TimeSpan.FromSeconds( (long)parameters[0] ) );

                if (parameters.Length == 2)
                {
                    _ = promise.Then( () =>
                    {
                        ( (LuaFunction)parameters[1] ).Call();
                    } ); 

                    return Promise.FromResult(new object[] { key } );
                }

                return promise.Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("delaygameobject", parameters =>
            {
                MultipleDelayBehaviour multipleDelayBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)parameters[0], new MultipleDelayBehaviour(TimeSpan.FromSeconds( (long)parameters[1] ) ), false);

                if (parameters.Length == 3)
                {
                    _ = multipleDelayBehaviour.Promise.Then( () =>
                    {
                        ( (LuaFunction)parameters[2] ).Call();
                    } );

                    return Promise.FromResult(new object[] { multipleDelayBehaviour.Key } );
                }

                return multipleDelayBehaviour.Promise.Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("canceldelay", parameters =>
            {
                bool canceled = Context.Current.Server.CancelQueueForExecution( (string)parameters[0] );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("containeradditem", parameters =>
            {
                return Context.Current.AddCommand(new ContainerAddItemCommand( (Container)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("containercreateitem", parameters =>
            {
                return Context.Current.AddCommand(new ContainerCreateItemCommand( (Container)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("containerremoveitem", parameters =>
            {
                return Context.Current.AddCommand(new ContainerRemoveItemCommand( (Container)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("containerreplaceitem", parameters =>
            {
                return Context.Current.AddCommand(new ContainerReplaceItemCommand( (Container)parameters[0], (Item)parameters[1], (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureaddcondition", parameters =>
            {
                return Context.Current.AddCommand(new CreatureAddConditionCommand( (Creature)parameters[0], ToCondition(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureattackarea", parameters =>
            {
                return Context.Current.AddCommand(new CreatureAttackAreaCommand( (Creature)parameters[0], (bool)parameters[1], ToPosition(parameters[2] ), ToOffsetArray(parameters[3] ), (ProjectileType?)(long?)parameters[4], (MagicEffectType?)(long?)parameters[5], ToAttack(parameters[6] ), ToCondition(parameters[7] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureattackcreature", parameters =>
            {
                return Context.Current.AddCommand(new CreatureAttackCreatureCommand( (Creature)parameters[0], (Creature)parameters[1], ToAttack(parameters[2] ), ToCondition(parameters[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureremovecondition", parameters =>
            {
                return Context.Current.AddCommand(new CreatureRemoveConditionCommand( (Creature)parameters[0], (ConditionSpecialCondition)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creaturedestroy", parameters =>
            {
                return Context.Current.AddCommand(new CreatureDestroyCommand( (Creature)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatedirection", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateDirectionCommand( (Creature)parameters[0], (Direction)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatehealth", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateHealthCommand( (Creature)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdateinvisible", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateInvisibleCommand( (Creature)parameters[0], (bool)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatelight", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateLightCommand( (Creature)parameters[0], ToLight(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdateoutfit", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateOutfitCommand( (Creature)parameters[0], ToOutfit(parameters[1] ), ToOutfit(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatepartyicon", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdatePartyIconCommand( (Creature)parameters[0], (PartyIcon)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdateskullicon", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSkullIconCommand( (Creature)parameters[0], (SkullIcon)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatespeed", parameters =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSpeedCommand( (Creature)parameters[0], (ushort)(long)parameters[1], (ushort)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creaturemove", parameters =>
            {
                return Context.Current.AddCommand(new CreatureMoveCommand( (Creature)parameters[0], ToTile(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("fluiditemupdatefluidtype", parameters =>
            {
                return Context.Current.AddCommand(new FluidItemUpdateFluidTypeCommand( (FluidItem)parameters[0], (FluidType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventoryadditem", parameters =>
            {
                return Context.Current.AddCommand(new InventoryAddItemCommand( (Inventory)parameters[0], (byte)(long)parameters[1], (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventorycreateitem", parameters =>
            {
                return Context.Current.AddCommand(new InventoryCreateItemCommand( (Inventory)parameters[0], (byte)(long)parameters[1], (ushort)(long)parameters[2], (byte)(long)parameters[3] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("inventoryremoveitem", parameters =>
            {
                return Context.Current.AddCommand(new InventoryRemoveItemCommand( (Inventory)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventoryreplaceitem", parameters =>
            {
                return Context.Current.AddCommand(new InventoryReplaceItemCommand( (Inventory)parameters[0], (Item)parameters[1], (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemclone", parameters =>
            {
                return Context.Current.AddCommand(new ItemCloneCommand( (Item)parameters[0], (bool)parameters[1] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("itemdecrement", parameters =>
            {
                return Context.Current.AddCommand(new ItemDecrementCommand( (Item)parameters[0], (byte)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemdestroy", parameters =>
            {
                return Context.Current.AddCommand(new ItemDestroyCommand( (Item)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemmove", parameters =>
            {
                return Context.Current.AddCommand(new ItemMoveCommand( (Item)parameters[0], (IContainer)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemtransform", parameters =>
            {
                return Context.Current.AddCommand(new ItemTransformCommand( (Item)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("monstersay", parameters =>
            {
                return Context.Current.AddCommand(new MonsterSayCommand( (Monster)parameters[0], (string)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("npcsay", parameters =>
            {
                return Context.Current.AddCommand(new NpcSayCommand( (Npc)parameters[0], (string)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                        
            lua.RegisterCoFunction("npcsaytoplayer", parameters =>
            {
                return Context.Current.AddCommand(new NpcSayToPlayerCommand( (Npc)parameters[0], (Player)parameters[1], (string)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("npctrade", parameters =>
            {
                List<OfferDto> offers = new List<OfferDto>();

                foreach (LuaTable item in ( (LuaTable)parameters[2] ).Values)
                {
                    string name = (string)item["name"];

                    ushort openTibiaId = (ushort)(long)item["item"];

                    byte type = (byte)(long)item["type"];

                    uint buyPrice = item["buyprice"] != null ? (uint)(long)item["buyprice"] : 0;

                    uint sellprice = item["sellprice"] != null ? (uint)(long)item["sellprice"] : 0;

                    ItemMetadata itemMetadata = server.ItemFactory.GetItemMetadataByOpenTibiaId(openTibiaId);

                    if (itemMetadata != null)
                    {
                        offers.Add(new OfferDto(itemMetadata.TibiaId, type, name ?? itemMetadata.Name, itemMetadata.Weight != null ? itemMetadata.Weight.Value : 0, buyPrice, sellprice) );                   
                    }
                }

                return Context.Current.AddCommand(new NpcTradeCommand( (Npc)parameters[0], (Player)parameters[1], offers) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("npcidle", parameters =>
            {
                if (server.Config.GameplayPrivateNpcSystem)
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>( (Npc)parameters[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Idle( (Player)parameters[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                else
                {
                    SingleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<SingleQueueNpcThinkBehaviour>( (Npc)parameters[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Idle( (Player)parameters[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                
                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("npcfarewell", parameters =>
            {
                if (server.Config.GameplayPrivateNpcSystem)
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>( (Npc)parameters[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Farewell( (Player)parameters[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                else
                {
                    SingleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<SingleQueueNpcThinkBehaviour>( (Npc)parameters[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Farewell( (Player)parameters[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("npcdisappear", parameters =>
            {
                if (server.Config.GameplayPrivateNpcSystem)
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>( (Npc)parameters[0]);

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Disappear( (Player)parameters[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                else
                {
                    SingleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<SingleQueueNpcThinkBehaviour>( (Npc)parameters[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Disappear( (Player)parameters[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playercreatemoney", parameters =>
            {
                return Context.Current.AddCommand(new PlayerCreateMoneyCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerdestroymoney", parameters =>
            {
                return Context.Current.AddCommand(new PlayerDestroyMoneyCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( (success) =>
                {
                    return success ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("playercountmoney", parameters =>
            {
                return Context.Current.AddCommand(new PlayerCountMoneyCommand( (Player)parameters[0] ) ).Then( (price) =>
                {
                    return Promise.FromResult(new object[] { price } );
                } );                  
            } );

            lua.RegisterCoFunction("playercreateitems", parameters =>
            {
                return Context.Current.AddCommand(new PlayerCreateItemsCommand( (Player)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2], (int)(long)parameters[3] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerdestroyitems", parameters =>
            {
                return Context.Current.AddCommand(new PlayerDestroyItemsCommand( (Player)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2], (int)(long)parameters[3] ) ).Then( (success) =>
                {
                    return success ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("playercountitems", parameters =>
            {
                return Context.Current.AddCommand(new PlayerCountItemsCommand( (Player)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( (count) =>
                {
                    return Promise.FromResult(new object[] { count } );
                } );                
            } );

            lua.RegisterCoFunction("playerachievement", parameters =>
            {
                return Context.Current.AddCommand(new PlayerAchievementCommand( (Player)parameters[0], (int)(long)parameters[1], (int)(long)parameters[2], (string)parameters[3] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateaxe", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateAxeCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateclub", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateClubCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatedistance", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateDistanceCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatefish", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateFishCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatefist", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateFistCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatemagiclevel", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateMagicLevelCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateshield", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateShieldCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatesword", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateSwordCommand( (Player)parameters[0], (byte)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playersay", parameters =>
            {
                return Context.Current.AddCommand(new PlayerSayCommand( (Player)parameters[0], (string)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playerupdatecapacity", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateCapacityCommand( (Player)parameters[0], (uint)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateexperience", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateExperienceCommand( (Player)parameters[0], (uint)(long)parameters[1], (ushort)(long)parameters[2], (byte)(long)parameters[3] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatemana", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateManaCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatesoul", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateSoulCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatestamina", parameters =>
            {
                return Context.Current.AddCommand(new PlayerUpdateStaminaCommand( (Player)parameters[0], (int)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerwhisper", parameters =>
            {
                return Context.Current.AddCommand(new PlayerWhisperCommand( (Player)parameters[0], (string)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playeryell", parameters =>
            {
                return Context.Current.AddCommand(new PlayerYellCommand( (Player)parameters[0], (string)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playergetoutfit", parameters =>
            {
                Player player = (Player)parameters[0];

                Addon addon;

                if (player.Outfits.TryGetOutfit( (ushort)(long)parameters[1], out addon) )
                {
                    return Promise.FromResult(new object[] { true, addon } );
                }

                return Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetoutfit", parameters =>
            {
                Player player = (Player)parameters[0];

                player.Outfits.SetOutfit( (ushort)(long)parameters[1], (Addon)(long)parameters[2] );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremoveoutfit", parameters =>
            {
                Player player = (Player)parameters[0];

                player.Outfits.RemoveOutfit( (ushort)(long)parameters[1] );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetstorage", parameters =>
            {
                Player player = (Player)parameters[0];

                int value;

                if (player.Storages.TryGetValue( (int)(long)parameters[1], out value) )
                {
                    return Promise.FromResult(new object[] { true, value } );
                }

                return Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetstorage", parameters =>
            {
                Player player = (Player)parameters[0];

                player.Storages.SetValue( (int)(long)parameters[1], (int)(long)parameters[2] );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovestorage", parameters =>
            {
                Player player = (Player)parameters[0];

                player.Storages.RemoveValue( (int)(long)parameters[1] );

                return Promise.FromResultAsEmptyObjectArray;
            } );
                      
            lua.RegisterCoFunction("playergetspells", parameters =>
            {
                Player player = (Player)parameters[0];
                
                return Promise.FromResult<object[]>(player.Spells.GetSpells().ToArray() );
            } );

            lua.RegisterCoFunction("playersetspell", parameters =>
            {
                Player player = (Player)parameters[0];

                player.Spells.SetSpell( (string)parameters[1] );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovespell", parameters =>
            {
                Player player = (Player)parameters[0];

                player.Spells.RemoveSpell( (string)parameters[1] );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("showanimatedtext", parameters =>
            {
                return Context.Current.AddCommand(new ShowAnimatedTextCommand(ToPosition(parameters[0] ), (AnimatedTextColor)(long)parameters[1], (string)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showmagiceffect", parameters =>
            {
                return Context.Current.AddCommand(new ShowMagicEffectCommand(ToPosition(parameters[0] ), (MagicEffectType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showprojectile", parameters =>
            {
                return Context.Current.AddCommand(new ShowProjectileCommand(ToPosition(parameters[0] ), ToPosition(parameters[1] ), (ProjectileType)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showtext", parameters =>
            {
                return Context.Current.AddCommand(new ShowTextCommand( (Creature)parameters[0], (TalkType)(long)parameters[1], (string)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showwindowtext", parameters =>
            {
                Context.Current.AddPacket( (Player)parameters[0], new ShowWindowTextOutgoingPacket( (TextColor)(long)parameters[1], (string)parameters[2] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("splashitemupdatefluidtype", parameters =>
            {
                return Context.Current.AddCommand(new SplashItemUpdateFluidTypeCommand( (SplashItem)parameters[0], (FluidType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                                               
            lua.RegisterCoFunction("stackableitemupdatecount", parameters =>
            {
                return Context.Current.AddCommand(new StackableItemUpdateCountCommand( (StackableItem)parameters[0], (byte)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileaddcreature", parameters =>
            {
                return Context.Current.AddCommand(new TileAddCreatureCommand( (Tile)parameters[0], (Creature)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileadditem", parameters =>
            {
                return Context.Current.AddCommand(new TileAddItemCommand( (Tile)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilecreateitem", parameters =>
            {
                return Context.Current.AddCommand(new TileCreateItemCommand( (Tile)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("tilecreateitemorincrement", parameters =>
            {
                return Context.Current.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)parameters[0], (ushort)(long)parameters[1], (byte)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilecreatemonster", parameters =>
            {
                return Context.Current.AddCommand(new TileCreateMonsterCommand( (Tile)parameters[0], (string)parameters[1] ) ).Then( (monster) =>
                {
                    return Promise.FromResult(new object[] { monster } );
                } );
            } );

            lua.RegisterCoFunction("tilecreatenpc", parameters =>
            {
                return Context.Current.AddCommand(new TileCreateNpcCommand( (Tile)parameters[0], (string)parameters[1] ) ).Then( (npc) =>
                {
                    return Promise.FromResult(new object[] { npc } );
                } );
            } );

            lua.RegisterCoFunction("tileremovecreature", parameters =>
            {
                return Context.Current.AddCommand(new TileRemoveCreatureCommand ( (Tile)parameters[0], (Creature)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileremoveitem", parameters =>
            {
                return Context.Current.AddCommand(new TileRemoveItemCommand ( (Tile)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilereplaceitem", parameters =>
            {
                return Context.Current.AddCommand(new TileReplaceItemCommand( (Tile)parameters[0], (Item)parameters[1], (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            this.server = server;
        }

        ~LuaScriptCollection()
        {
            Dispose(false);
        }

        private IServer server;

#if AOT
        [RequiresUnreferencedCode("Used by lua.RegisterFunction.")]
#endif
        public void Print(params object[] parameters)
        {
            server.Logger.WriteLine(string.Join("\t", parameters), LogLevel.Information);
        }

#if AOT
        [RequiresUnreferencedCode("Used by lua.RegisterFunction.")]
#endif
        public string TypeOf(object obj)
        {
            return obj.GetType().FullName;
        }

#if AOT
        [RequiresUnreferencedCode("Used by lua.RegisterFunction.")]
#endif
        public object Cast(object obj, string typeName)
        {
            return Convert.ChangeType(obj, server.PluginLoader.GetType(typeName) );
        }

#if AOT
        [RequiresUnreferencedCode("Used by lua.RegisterFunction.")]
#endif
        public object GetConfig(string file, string key)
        {
            if (file == "server")
            {
                return server.Config.GetValue(key);
            }
            else if (file == "quests")
            {
                return server.Quests.GetValue(key);
            }
            else if (file == "outfits")
            {
                return server.Outfits.GetValue(key);
            }
            else if (file == "plugins")
            {
                return server.Plugins.GetValue(key);
            }

            return null;
        }

#if AOT
        [RequiresUnreferencedCode("Used by lua.RegisterFunction.")]
#endif
        public string GetFullPath(string relativePath)
        {
            return server.PathResolver.GetFullPath(relativePath);
        }
        
        /// <exception cref="ArgumentException"></exception>
       
        private Light ToLight(object parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter is Light)
            {
                return (Light)parameter;
            }
            
            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                return new Light( (byte)(long)table["level"], (byte)(long)table["color"] );
            }
         
            throw new ArgumentException("Parameter must be Light or LuaTable with level and color.");
        }

        /// <exception cref="ArgumentException"></exception>

        private Outfit ToOutfit(object parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter is Outfit)
            {
                return (Outfit)parameter;
            }
            
            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                if (table["itemid"] != null)
                {
                    return new Outfit( (ushort)(long)table["itemid"] );
                }

                return new Outfit( (ushort)(long)table["id"], (byte)(long)table["head"], (byte)(long)table["body"], (byte)(long)table["legs"], (byte)(long)table["feet"], (Addon)(long)table["addon"] );
            }
         
            throw new ArgumentException("Parameter must be Outfit or LuaTable with itemid or with id, head, body, legs, feet and addon.");
        }

        /// <exception cref="ArgumentException"></exception>

        private Offset ToOffset(object parameter)
        {
            if (parameter is Offset)
            {
                return (Offset)parameter;
            }

            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                return new Offset( (int)(long)table[1], (int)(long)table[2] );
            }
            
            throw new ArgumentException("Parameter must be Offset or LuaTable with two values, one for x and other for y.");
        }

        /// <exception cref="ArgumentException"></exception>

        private Offset[] ToOffsetArray(object parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter is Offset[] )
            {
                return (Offset[] )parameter;
            }

            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                return table.Values.Cast<LuaTable>().Select(v => ToOffset(v) ).ToArray();
            }

            throw new ArgumentException("Parameter must be array of Offset or array of LuaTable with two items, one for x and other for y.");
        }

        /// <exception cref="ArgumentException"></exception>

        private Position ToPosition(object parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter is Position)
            {
                return (Position)parameter;
            }

            if (parameter is IContent content)
            {
                Position position = null;

                switch (content)
                {
                    case Item item:

                        switch (item.Root() )
                        {
                            case Tile tile:

                                position = tile.Position;

                                break;

                            case Inventory inventory:

                                position = inventory.Player.Tile.Position;

                                break;

                            case Safe safe:

                                position = safe.Player.Tile.Position;

                                break;
                        }

                        break;

                    case Creature creature:

                        position = creature.Tile.Position;

                        break;
                }

                return position;
            }

            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;
                
                return new Position( (int)(long)table["x"], (int)(long)table["y"], (int)(long)table["z"] );
            }
         
            throw new ArgumentException("Parameter must by Position or LuaTable with x, y and z.");
        }

        /// <exception cref="ArgumentException"></exception>

        private Tile ToTile(object parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter is Tile)
            {
                return (Tile)parameter;
            }
            
            if (parameter is LuaTable)
            {
                return Context.Current.Server.Map.GetTile(ToPosition(parameter) );
            }
         
            throw new ArgumentException("Parameter must by Tile or LuaTable with x, y and z.");
        }

        /// <exception cref="ArgumentException"></exception>

        private Attack ToAttack(object parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter is Attack)
            {
                return (Attack)parameter;
            }
            
            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                switch ( (string)table["type"] )
                {
                    case "distance":

                        return new DistanceAttack( (ProjectileType)(long)table["projectiletype"], (int)(long)table["min"], (int)(long)table["max"] );

                    case "healing":

                        return new HealingAttack( (MagicEffectType?)(long?)table["magiceffecttype"], (int)(long)table["min"], (int)(long)table["max"] );

                    case "melee":

                        return new MeleeAttack( (int)(long)table["min"], (int)(long)table["max"] );

                    case "simple":

                        return new SimpleAttack( (ProjectileType?)(long?)table["projectiletype"], (MagicEffectType?)(long?)table["magiceffecttype"], (AnimatedTextColor?)(long?)table["animatedtextcolor"], (int)(long)table["min"], (int)(long)table["max"] );
                }
            }

            throw new ArgumentException("Parameter must by Attack or LuaTable with type, projectiletype, magiceffecttype, animatedtextcolor, min and max.");
        }

        /// <exception cref="ArgumentException"></exception>

        private Condition ToCondition(object parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            if (parameter is Condition)
            {
                return (Condition)parameter;
            }
            
            if (parameter is LuaTable)
            {
                LuaTable table = (LuaTable)parameter;

                switch ( (string)table["type"] )
                {
                    case "damage":

                        return new DamageCondition( (SpecialCondition)(long)table["specialcondition"], (MagicEffectType?)(long?)table["magiceffecttype"], (AnimatedTextColor?)(long?)table["animatedtextcolor"], ( (LuaTable)table["damages"] ).Values.Cast<long>().Select(v => (int)v ).ToArray(), TimeSpan.FromSeconds( (int)(long)table["interval"] ) );

                    case "drowning":

                        return new DrowningCondition( (int)(long)table["damage"], TimeSpan.FromSeconds( (int)(long)table["interval"] ) );

                    case "haste":

                        return new HasteCondition( (ushort)(long)table["speed"], TimeSpan.FromSeconds( (int)(long)table["duration"] ) );

                    case "invisible":

                        return new InvisibleCondition( TimeSpan.FromSeconds( (int)(long)table["duration"] ) );

                    case "light":

                        return new LightCondition(ToLight(table["light"] ), TimeSpan.FromSeconds( (int)(long)table["duration"] ) );

                    case "magicshield":

                        return new MagicShieldCondition( TimeSpan.FromSeconds( (int)(long)table["duration"] ) );

                    case "outfit":

                        return new OutfitCondition(ToOutfit(table["outfit"] ), TimeSpan.FromSeconds( (int)(long)table["duration"] ) );

                    case "regeneration":

                       return new RegenerationCondition( (int)(long)table["regenerationtick"] );

                }
            }
         
            throw new ArgumentException("Parameter must by Condition or LuaTable with type, specialcondition, magiceffecttype, animatedtextcolor, damages, interval, damage, speed, duration, light, outfit and/or regenerationtick.");
        }

        private Dictionary<string, string> chunks = new Dictionary<string, string>();

        public string GetChunk(string path)
        {
            string chunk;

            if ( !chunks.TryGetValue(path, out chunk) )
            {
                chunk = File.ReadAllText(path);

                chunks.Add(path, chunk);
            }

            return chunk;
        }

        private Dictionary<string, LuaScope> libs = new Dictionary<string, LuaScope>();

        public bool TryGetLib(string libPath, out LuaScope lib)
        {
            return libs.TryGetValue(libPath, out lib);
        }

        public LuaScope LoadLib(string libPath, Func<LuaScope> loadParent)
        {
            LuaScope lib;

            if ( !TryGetLib(libPath, out lib) )
            {
                LuaScope parent = loadParent();

                if (parent == null)
                {
                    parent = lua;
                }

                lib = parent.LoadNewChunk(GetChunk(libPath), libPath);

                libs.Add(libPath, lib);
            }

            return lib;
        }

        public LuaScope LoadLib(params string[] libPaths)
        {
            LuaScope Load(int i)
            {
                if (i > libPaths.Length - 1)
                {
                    return null;
                }

                return LoadLib(libPaths[i], () => Load(i + 1) );
            }

            return Load(0);
        }

        public LuaScope LoadScript(string scriptPath, LuaScope parent)
        {
            if (parent == null)
            {
                parent = lua;
            }
                
            return parent.LoadNewChunk(GetChunk(scriptPath), scriptPath);
        }

        public LuaScope LoadScript(params string[] scriptPathAndLibPaths)
        {
            return LoadScript(scriptPathAndLibPaths[0], LoadLib(scriptPathAndLibPaths[1..] ) );
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    if (libs != null)
                    {
                        foreach (var lib in libs)
                        {
                            lib.Value.Dispose();
                        }
                    }

                    if (lua != null)
                    {
                        lua.Dispose();
                    }
                }
            }
        }
    }
}