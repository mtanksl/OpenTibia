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
        private IServer server;

        public LuaScriptCollection(IServer server)
        {
            this.server = server;
        }

        ~LuaScriptCollection()
        {
            Dispose(false);
        }

        private ILuaScope lua;

        public void Start()
        {
            lua = new LuaScope(server);

            lua.RegisterFunction("print", this, GetType().GetMethod(nameof(Print) ) );

            lua.RegisterFunction("typeof", this, GetType().GetMethod(nameof(TypeOf) ) );

            lua.RegisterFunction("cast", this, GetType().GetMethod(nameof(Cast) ) );

            lua.RegisterFunction("getconfig", this, GetType().GetMethod(nameof(GetConfig) ) );

            lua.RegisterFunction("getfullpath", this, GetType().GetMethod(nameof(GetFullPath) ) );
            
            lua.RegisterCoFunction("waithandle", (luaScope, parameters) =>
            {
                var promise = new PromiseResult<object[]>();

                return Promise.FromResult(new object[] { promise } );
            } );

            lua.RegisterCoFunction("wait", (luaScope, parameters) =>
            {
                var promise = (PromiseResult<object[]>)parameters[0];

                return promise.Then(result =>
                {
                    return Promise.FromResult(result);
                } ); 
            } );

            lua.RegisterCoFunction("set", (luaScope, parameters) =>
            {
                var promise = (PromiseResult<object[]>)parameters[0];

                promise.TrySetResult(parameters.Skip(1).ToArray() );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("delay", (luaScope, parameters) =>
            {             
                if (parameters[0] is GameObject)
                {
                    MultipleDelayBehaviour multipleDelayBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)parameters[0], new MultipleDelayBehaviour(TimeSpan.FromMilliseconds(LuaScope.GetInt64(parameters[1] ) ) ), false);

                    if (parameters.Length == 3)
                    {
                        _ = multipleDelayBehaviour.Promise.Then( () =>
                        {
                            return luaScope.CallFunction( (LuaFunction)parameters[2] ); // Ignore result
                        } );

                        return Promise.FromResult(new object[] { multipleDelayBehaviour.Key } );
                    }

                    return multipleDelayBehaviour.Promise.Then( () =>
                    {
                        return Promise.FromResultAsEmptyObjectArray;
                    } );
                }
                else
                {
                    string key = Guid.NewGuid().ToString();

                    Promise promise = Promise.Delay(key, TimeSpan.FromMilliseconds(LuaScope.GetInt64(parameters[0] ) ) );

                    if (parameters.Length == 2)
                    {
                        _ = promise.Then( () =>
                        {
                            return luaScope.CallFunction( (LuaFunction)parameters[1] ); // Ignore result
                        } ); 

                        return Promise.FromResult(new object[] { key } );
                    }

                    return promise.Then( () =>
                    {
                        return Promise.FromResultAsEmptyObjectArray;
                    } );
                }                                
            } );

            lua.RegisterCoFunction("canceldelay", (luaScope, parameters) =>
            {
                bool canceled = Context.Current.Server.CancelQueueForExecution(LuaScope.GetString(parameters[0] ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("eventhandler", (luaScope, parameters) =>
            {
                if (parameters[0] is GameObject)
                {
                    MultipleEventHandlerBehaviour multipleEventHandlerBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)parameters[0], new MultipleEventHandlerBehaviour(Type.GetType(LuaScope.GetString(parameters[1] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)parameters[2], e); // Ignore result

                    } ), false);

                    return Promise.FromResult(new object[] { multipleEventHandlerBehaviour.Key.ToString() } );
                }
                else
                {
                    Guid key = Context.Current.Server.EventHandlers.Subscribe(Type.GetType(LuaScope.GetString(parameters[0] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)parameters[1], e); // Ignore result
                    } );

                    return Promise.FromResult(new object[] { key.ToString() } );
                }
            } );

            lua.RegisterCoFunction("canceleventhandler", (luaScope, parameters) =>
            {
                bool canceled = Context.Current.Server.EventHandlers.Unsubscribe(Guid.Parse(LuaScope.GetString(parameters[0] ) ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("gameobjecteventhandler", (luaScope, parameters) =>
            {
                if (parameters[0] is GameObject)
                {
                    MultipleGameObjectEventHandlerBehaviour multipleEventHandlerBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)parameters[0], new MultipleGameObjectEventHandlerBehaviour( (GameObject)parameters[1], Type.GetType(LuaScope.GetString(parameters[2] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)parameters[3], e); // Ignore result

                    } ), false);

                    return Promise.FromResult(new object[] { multipleEventHandlerBehaviour.Key.ToString() } );
                }
                else
                {
                    Guid key = Context.Current.Server.GameObjectEventHandlers.Subscribe( (GameObject)parameters[0], Type.GetType(LuaScope.GetString(parameters[1] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)parameters[2], e); // Ignore result
                    } );

                    return Promise.FromResult(new object[] { key.ToString() } );
                }
            } );

            lua.RegisterCoFunction("gameobjectcanceleventhandler", (luaScope, parameters) =>
            {
                bool canceled = Context.Current.Server.GameObjectEventHandlers.Unsubscribe(Guid.Parse(LuaScope.GetString(parameters[0] ) ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("positionaleventhandler", (luaScope, parameters) =>
            {
                if (parameters[0] is GameObject)
                {
                    MultiplePositionalEventHandlerBehaviour multipleEventHandlerBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)parameters[0], new MultiplePositionalEventHandlerBehaviour( (GameObject)parameters[1], Type.GetType(LuaScope.GetString(parameters[2] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)parameters[3], e); // Ignore result

                    } ), false);

                    return Promise.FromResult(new object[] { multipleEventHandlerBehaviour.Key.ToString() } );
                }
                else
                {
                    Guid key = Context.Current.Server.PositionalEventHandlers.Subscribe( (GameObject)parameters[0], Type.GetType(LuaScope.GetString(parameters[1] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)parameters[2], e); // Ignore result
                    } );

                    return Promise.FromResult(new object[] { key.ToString() } );
                }
            } );

            lua.RegisterCoFunction("positionalcanceleventhandler", (luaScope, parameters) =>
            {
                bool canceled = Context.Current.Server.PositionalEventHandlers.Unsubscribe(Guid.Parse(LuaScope.GetString(parameters[0] ) ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("containeradditem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ContainerAddItemCommand( (Container)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("containercreateitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ContainerCreateItemCommand( (Container)parameters[0], LuaScope.GetUInt16(parameters[1] ), LuaScope.GetByte(parameters[2] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("containerremoveitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ContainerRemoveItemCommand( (Container)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("containerreplaceitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ContainerReplaceItemCommand( (Container)parameters[0], (Item)parameters[1], (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureaddcondition", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureAddConditionCommand( (Creature)parameters[0], ToCondition(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureattackarea", (luaScope, parameters) =>
            {
                if (parameters.Length == 8)
                {
                    return Context.Current.AddCommand(new CreatureAttackAreaCommand( (Creature)parameters[0], (bool)parameters[1], ToPosition(parameters[2] ), ToOffsetArray(parameters[3] ), (ProjectileType?)(long?)parameters[4], (MagicEffectType?)(long?)parameters[5], ToAttack(parameters[6] ), ToCondition(parameters[7] ) ) ).Then( () =>
                    {
                        return Promise.FromResultAsEmptyObjectArray;
                    } );
                }

                return Context.Current.AddCommand(new CreatureAttackAreaCommand( (Creature)parameters[0], (bool)parameters[1], ToPosition(parameters[2] ), ToOffsetArray(parameters[3] ), (ProjectileType?)(long?)parameters[4], (MagicEffectType?)(long?)parameters[5], LuaScope.GetUInt16(parameters[6] ), LuaScope.GetByte(parameters[7] ), ToAttack(parameters[8] ), ToCondition(parameters[9] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureattackcreature", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureAttackCreatureCommand( (Creature)parameters[0], (Creature)parameters[1], ToAttack(parameters[2] ), ToCondition(parameters[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureremovecondition", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureRemoveConditionCommand( (Creature)parameters[0], (ConditionSpecialCondition)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creaturedestroy", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureDestroyCommand( (Creature)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatedirection", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateDirectionCommand( (Creature)parameters[0], (Direction)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatehealth", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateHealthCommand( (Creature)parameters[0], LuaScope.GetInt32(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdateinvisible", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateInvisibleCommand( (Creature)parameters[0], (bool)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatelight", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateLightCommand( (Creature)parameters[0], ToLight(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdateoutfit", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateOutfitCommand( (Creature)parameters[0], ToOutfit(parameters[1] ), ToOutfit(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatespeed", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSpeedCommand( (Creature)parameters[0], LuaScope.GetUInt16(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creaturemove", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new CreatureMoveCommand( (Creature)parameters[0], ToTile(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("fluiditemupdatefluidtype", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new FluidItemUpdateFluidTypeCommand( (FluidItem)parameters[0], (FluidType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventoryadditem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new InventoryAddItemCommand( (Inventory)parameters[0], LuaScope.GetByte(parameters[1] ), (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventorycreateitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new InventoryCreateItemCommand( (Inventory)parameters[0], LuaScope.GetByte(parameters[1] ), LuaScope.GetUInt16(parameters[2] ), LuaScope.GetByte(parameters[3] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("inventoryremoveitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new InventoryRemoveItemCommand( (Inventory)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventoryreplaceitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new InventoryReplaceItemCommand( (Inventory)parameters[0], (Item)parameters[1], (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemclone", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ItemCloneCommand( (Item)parameters[0], (bool)parameters[1] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("itemdecrement", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ItemDecrementCommand( (Item)parameters[0], LuaScope.GetByte(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemdestroy", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ItemDestroyCommand( (Item)parameters[0] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemmove", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ItemMoveCommand( (Item)parameters[0], (IContainer)parameters[1], LuaScope.GetByte(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemtransform", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ItemTransformCommand( (Item)parameters[0], LuaScope.GetUInt16(parameters[1] ), LuaScope.GetByte(parameters[2] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("monstersay", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new MonsterSayCommand( (Monster)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("monsteryell", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new MonsterYellCommand( (Monster)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("npcsay", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new NpcSayCommand( (Npc)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                        
            lua.RegisterCoFunction("npcsaytoplayer", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new NpcSayToPlayerCommand( (Npc)parameters[0], (Player)parameters[1], LuaScope.GetString(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("npctrade", (luaScope, parameters) =>
            {
                List<OfferDto> offers = new List<OfferDto>();

                foreach (LuaTable item in ( (LuaTable)parameters[2] ).Values)
                {
                    string name = LuaScope.GetString(item["name"] );

                    ushort openTibiaId = LuaScope.GetUInt16(item["item"] );

                    byte type = LuaScope.GetByte(item["type"] );

                    uint buyPrice = item["buyprice"] != null ? LuaScope.GetUInt32(item["buyprice"] ) : 0;

                    uint sellprice = item["sellprice"] != null ? LuaScope.GetUInt32(item["sellprice"] ) : 0;

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

            lua.RegisterCoFunction("npcidle", (luaScope, parameters) =>
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

            lua.RegisterCoFunction("npcfarewell", (luaScope, parameters) =>
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

            lua.RegisterCoFunction("npcdisappear", (luaScope, parameters) =>
            {
                if (server.Config.GameplayPrivateNpcSystem)
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>( (Npc)parameters[0] );

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

            lua.RegisterCoFunction("playercreatemoney", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerCreateMoneyCommand( (Player)parameters[0], LuaScope.GetInt32(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerdestroymoney", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerDestroyMoneyCommand( (Player)parameters[0], LuaScope.GetInt32(parameters[1] ) ) ).Then( (success) =>
                {
                    return success ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("playercountmoney", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerCountMoneyCommand( (Player)parameters[0] ) ).Then( (price) =>
                {
                    return Promise.FromResult(new object[] { price } );
                } );                  
            } );

            lua.RegisterCoFunction("playercreateitems", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerCreateItemsCommand( (Player)parameters[0], LuaScope.GetUInt16(parameters[1] ), LuaScope.GetByte(parameters[2] ), LuaScope.GetInt32(parameters[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerdestroyitems", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerDestroyItemsCommand( (Player)parameters[0], LuaScope.GetUInt16(parameters[1] ), LuaScope.GetByte(parameters[2] ), LuaScope.GetInt32(parameters[3] ) ) ).Then( (success) =>
                {
                    return success ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("playercountitems", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerCountItemsCommand( (Player)parameters[0], LuaScope.GetUInt16(parameters[1] ), LuaScope.GetByte(parameters[2] ) ) ).Then( (count) =>
                {
                    return Promise.FromResult(new object[] { count } );
                } );                
            } );

            lua.RegisterCoFunction("playerachievement", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerAchievementCommand( (Player)parameters[0], LuaScope.GetInt32(parameters[1] ), LuaScope.GetInt32(parameters[2] ), LuaScope.GetString(parameters[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerbless", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerBlessCommand( (Player)parameters[0], LuaScope.GetString(parameters[1] ), LuaScope.GetString(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playeraddexperience", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerAddExperienceCommand( (Player)parameters[0], LuaScope.GetUInt64(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerremoveexperience", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerRemoveExperienceCommand( (Player)parameters[0], LuaScope.GetUInt64(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playeraddskillpoints", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerAddSkillPointsCommand( (Player)parameters[0], (Skill)(long)parameters[1], LuaScope.GetUInt64(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerremoveskillpoints", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerRemoveSkillPointsCommand( (Player)parameters[0], (Skill)(long)parameters[1], LuaScope.GetUInt64(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateskill", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateSkillCommand( (Player)parameters[0], (Skill)(long)parameters[1], LuaScope.GetUInt64(parameters[2] ), LuaScope.GetByte(parameters[3] ), LuaScope.GetByte(parameters[4] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                        
            lua.RegisterCoFunction("playersay", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerSayCommand( (Player)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playerupdatecapacity", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateCapacityCommand( (Player)parameters[0], LuaScope.GetUInt32(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateexperience", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateExperienceCommand( (Player)parameters[0], LuaScope.GetUInt64(parameters[1] ), LuaScope.GetUInt16(parameters[2] ), LuaScope.GetByte(parameters[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatemana", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateManaCommand( (Player)parameters[0], LuaScope.GetInt32(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatesoul", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateSoulCommand( (Player)parameters[0], LuaScope.GetInt32(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatestamina", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateStaminaCommand( (Player)parameters[0], LuaScope.GetInt32(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerwhisper", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerWhisperCommand( (Player)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playeryell", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new PlayerYellCommand( (Player)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playergetoutfit", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                Addon addon;

                if (player.Outfits.TryGetOutfit(LuaScope.GetUInt16(parameters[1] ), out addon) )
                {
                    return Promise.FromResult(new object[] { true, addon } );
                }

                return Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetoutfit", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Outfits.SetOutfit(LuaScope.GetUInt16(parameters[1] ), (Addon)(long)parameters[2] );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremoveoutfit", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Outfits.RemoveOutfit(LuaScope.GetUInt16(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetstorage", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                int value;

                if (player.Storages.TryGetValue(LuaScope.GetInt32(parameters[1] ), out value) )
                {
                    return Promise.FromResult(new object[] { true, value } );
                }

                return Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetstorage", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Storages.SetValue(LuaScope.GetInt32(parameters[1] ), LuaScope.GetInt32(parameters[2] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovestorage", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Storages.RemoveValue(LuaScope.GetInt32(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );
            
            lua.RegisterCoFunction("playergetachievements", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                string[] achievements = player.Achievements.GetAchievements().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(achievements) } );
            } );

            lua.RegisterCoFunction("playerhasachievement", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                return player.Achievements.HasAchievement(LuaScope.GetString(parameters[1] ) ) ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetachievement", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Achievements.SetAchievement(LuaScope.GetString(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremoveachievement", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Achievements.RemoveAchievement(LuaScope.GetString(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetspells", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                string[] spells = player.Spells.GetSpells().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(spells) } );
            } );

            lua.RegisterCoFunction("playerhasspell", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                return player.Spells.HasSpell(LuaScope.GetString(parameters[1] ) ) ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetspell", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Spells.SetSpell(LuaScope.GetString(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovespell", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Spells.RemoveSpell(LuaScope.GetString(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetblesses", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                string[] blesses = player.Blesses.GetBlesses().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(blesses) } );
            } );

            lua.RegisterCoFunction("playerhasbless", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                return player.Blesses.HasBless(LuaScope.GetString(parameters[1] ) ) ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetbless", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Blesses.SetBless(LuaScope.GetString(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovebless", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                player.Blesses.RemoveBless(LuaScope.GetString(parameters[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerstopwalk", (luaScope, parameters) =>
            {
                Player player = (Player)parameters[0];

                Context.Current.AddPacket(player, new StopWalkOutgoingPacket(player.Direction) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("showanimatedtext", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ShowAnimatedTextCommand(ToPosition(parameters[0] ), (AnimatedTextColor)(long)parameters[1], LuaScope.GetString(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showmagiceffect", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ShowMagicEffectCommand(ToPosition(parameters[0] ), (MagicEffectType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showprojectile", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ShowProjectileCommand(ToPosition(parameters[0] ), ToPosition(parameters[1] ), (ProjectileType)(long)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showtext", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new ShowTextCommand( (Creature)parameters[0], (TalkType)(long)parameters[1], LuaScope.GetString(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showwindowtext", (luaScope, parameters) =>
            {
                Context.Current.AddPacket( (Player)parameters[0], new ShowWindowTextOutgoingPacket( (TextColor)(long)parameters[1], LuaScope.GetString(parameters[2] ) ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("splashitemupdatefluidtype", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new SplashItemUpdateFluidTypeCommand( (SplashItem)parameters[0], (FluidType)(long)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                                               
            lua.RegisterCoFunction("stackableitemupdatecount", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new StackableItemUpdateCountCommand( (StackableItem)parameters[0], LuaScope.GetByte(parameters[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileaddcreature", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileAddCreatureCommand( (Tile)parameters[0], (Creature)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileadditem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileAddItemCommand( (Tile)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilecreateitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileCreateItemCommand( (Tile)parameters[0], LuaScope.GetUInt16(parameters[1] ), LuaScope.GetByte(parameters[2] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("tilecreateitemorincrement", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)parameters[0], LuaScope.GetUInt16(parameters[1] ), LuaScope.GetByte(parameters[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilecreatemonster", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileCreateMonsterCommand( (Tile)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( (monster) =>
                {
                    return Promise.FromResult(new object[] { monster } );
                } );
            } );

            lua.RegisterCoFunction("tilecreatenpc", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileCreateNpcCommand( (Tile)parameters[0], LuaScope.GetString(parameters[1] ) ) ).Then( (npc) =>
                {
                    return Promise.FromResult(new object[] { npc } );
                } );
            } );

            lua.RegisterCoFunction("tileremovecreature", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileRemoveCreatureCommand ( (Tile)parameters[0], (Creature)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileremoveitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileRemoveItemCommand ( (Tile)parameters[0], (Item)parameters[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilereplaceitem", (luaScope, parameters) =>
            {
                return Context.Current.AddCommand(new TileReplaceItemCommand( (Tile)parameters[0], (Item)parameters[1], (Item)parameters[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("mapgettownbyname", (luaScope, parameters) =>
            {
                Town town = Context.Current.Server.Map.GetTown(LuaScope.GetString(parameters[0] ) );

                return Promise.FromResult(new object[] { town } );
            } );

            lua.RegisterCoFunction("mapgetwaypointbyname", (luaScope, parameters) =>
            {
                Waypoint waypoint = Context.Current.Server.Map.GetWaypoint(LuaScope.GetString(parameters[0] ) );

                return Promise.FromResult(new object[] { waypoint } );
            } );

            lua.RegisterCoFunction("mapgethousebyname", (luaScope, parameters) =>
            {
                House house = Context.Current.Server.Map.GetHouse(LuaScope.GetString(parameters[0] ) );

                return Promise.FromResult(new object[] { house } );
            } );

            lua.RegisterCoFunction("mapgettile", (luaScope, parameters) =>
            {
                Tile tile = Context.Current.Server.Map.GetTile(ToPosition(parameters[0] ) );

                return Promise.FromResult(new object[] { tile } );
            } );

            lua.RegisterCoFunction("mapgetobserversoftypeplayer", (luaScope, parameters) =>
            {
                Player[] players = Context.Current.Server.Map.GetObserversOfTypePlayer(ToPosition(parameters[0] ) ).ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(players) } );
            } );

            lua.RegisterCoFunction("gameobjectsgetplayers", (luaScope, parameters) =>
            {
                Player[] players = Context.Current.Server.GameObjects.GetPlayers().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(players) } );
            } );

            lua.RegisterCoFunction("gameobjectsgetplayerbyname", (luaScope, parameters) =>
            {
                Player player = Context.Current.Server.GameObjects.GetPlayerByName(LuaScope.GetString(parameters[0] ) );

                return Promise.FromResult(new object[] { player } );
            } );
        }

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
            if (file == "values")
            {
                return server.Values.GetValue(key);
            }
            else if (file == "server")
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
            else if (file == "vocations")
            {
                return server.Vocations.GetValue(key);
            }
            else if (file == "plugins")
            {
                return server.Plugins.GetValue(key);
            }
            else if (file == "scripts")
            {
                return server.Scripts.GetValue(key);
            }
            else if (file == "gameobjectscripts")
            {
                return server.GameObjectScripts.GetValue(key);
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

                return new Light(LuaScope.GetByte(table["level"] ), LuaScope.GetByte(table["color"] ) );
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
                    return new Outfit(LuaScope.GetUInt16(table["itemid"] ) );
                }

                return new Outfit(LuaScope.GetUInt16(table["id"] ), LuaScope.GetByte(table["head"] ), LuaScope.GetByte(table["body"] ), LuaScope.GetByte(table["legs"] ), LuaScope.GetByte(table["feet"] ), (Addon)(long)table["addon"] );
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

                return new Offset(LuaScope.GetInt32(table[1] ), LuaScope.GetInt32(table[2] ) );
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
                
                return new Position(LuaScope.GetInt32(table["x"] ), LuaScope.GetInt32(table["y"] ), LuaScope.GetInt32(table["z"] ) );
            }
         
            throw new ArgumentException("Parameter must be Position or LuaTable with x, y and z.");
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
         
            throw new ArgumentException("Parameter must be Tile or LuaTable with x, y and z.");
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

                switch (LuaScope.GetString(table["type"] ) )
                {
                    case "damage":

                        return new DamageAttack( (ProjectileType?)(long?)table["projectiletype"], (MagicEffectType?)(long?)table["magiceffecttype"], (DamageType)(long)table["damagetype"], LuaScope.GetInt32(table["min"] ), LuaScope.GetInt32(table["max"] ) );
                    
                    case "healing":
                    
                        return new HealingAttack(LuaScope.GetInt32(table["min"] ), LuaScope.GetInt32(table["max"] ) );
                }
            }

            throw new ArgumentException("Parameter must be Attack or LuaTable with type, projectiletype, magiceffecttype, damagetype, min and/or max.");
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

                switch (LuaScope.GetString(table["type"] ) )
                {
                    case "damage":

                        return new DamageCondition( (SpecialCondition)(long)table["specialcondition"], (MagicEffectType?)(long?)table["magiceffecttype"], (DamageType)(long)table["damagetype"], ( (LuaTable)table["damages"] ).Values.Cast<long>().Select(v => (int)v ).ToArray(), TimeSpan.FromSeconds(LuaScope.GetInt32(table["interval"] ) ) );

                    case "drowning":

                        return new DrowningCondition(LuaScope.GetInt32(table["damage"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["interval"] ) ) );

                    case "drunk":

                        return new DrunkCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "haste":

                        return new HasteCondition(LuaScope.GetUInt16(table["speed"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "invisible":

                        return new InvisibleCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "light":

                        return new LightCondition(ToLight(table["light"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "logoutblock":

                        return new LogoutBlockCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "magicshield":

                        return new MagicShieldCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "outfit":

                        return new OutfitCondition(ToOutfit(table["outfit"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );
                               
                    case "protectionzoneblock":

                        return new ProtectionZoneBlockCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "slowed":

                        return new SlowedCondition(LuaScope.GetUInt16(table["speed"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );
                }
            }
         
            throw new ArgumentException("Parameter must be Condition or LuaTable with type, specialcondition, magiceffecttype, animatedtextcolor, damages, interval, damage, speed, duration, light and/or outfit.");
        }

        public string GetChunk(string path)
        {
            string chunk = File.ReadAllText(path);

            return chunk;
        }

        private Dictionary<string, ILuaScope> libs = new Dictionary<string, ILuaScope>();

        /// <exception cref="ObjectDisposedException"></exception>
     
        public bool TryGetLib(string libPath, out ILuaScope lib)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScriptCollection) );
            }

            return libs.TryGetValue(libPath, out lib);
        }

        /// <exception cref="ObjectDisposedException"></exception>
      
        public ILuaScope LoadLib(string libPath, Func<ILuaScope> loadParent)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScriptCollection) );
            }

            ILuaScope lib;

            if ( !TryGetLib(libPath, out lib) )
            {
                ILuaScope parent = loadParent();

                if (parent == null)
                {
                    parent = lua;
                }

                lib = parent.LoadNewChunk(GetChunk(libPath), libPath);

                libs.Add(libPath, lib);
            }

            return lib;
        }

        /// <exception cref="ObjectDisposedException"></exception>
     
        public ILuaScope LoadLib(params string[] libPaths)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScriptCollection) );
            }

            ILuaScope Load(int i)
            {
                if (i > libPaths.Length - 1)
                {
                    return null;
                }

                return LoadLib(libPaths[i], () => Load(i + 1) );
            }

            return Load(0);
        }

        /// <exception cref="ObjectDisposedException"></exception>
     
        public ILuaScope LoadScript(string scriptPath, ILuaScope parent)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScriptCollection) );
            }

            if (parent == null)
            {
                parent = lua;
            }

            return parent.LoadNewChunk(GetChunk(scriptPath), scriptPath);
        }

        /// <exception cref="ObjectDisposedException"></exception>
     
        public ILuaScope LoadScript(params string[] scriptPathAndLibPaths)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LuaScriptCollection) );
            }

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