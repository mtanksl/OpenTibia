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

            lua.RegisterCoFunction("registerplugin", (luaScope, args) =>
            {
                string nodeType = LuaScope.GetString(args[0] );

                LuaTable parameters = (LuaTable)args[1];

                if (nodeType == "actions")
                {
                    server.Plugins.ParseActions(null, luaScope, parameters);
                }
                else if (nodeType == "movements")
                {
                    server.Plugins.ParseMovements(null, luaScope, parameters);
                }
                else if (nodeType == "talkactions")
                {
                    server.Plugins.ParseTalkActions(null, luaScope, parameters);                        
                }
                else if (nodeType == "creaturescripts")
                {
                    server.Plugins.ParseCreatureScripts(null, luaScope, parameters);              
                }                    
                else if (nodeType == "globalevents")
                {
                    server.Plugins.ParseCreatureGlobalEvents(null, luaScope, parameters);
                }
                else if (nodeType == "items")
                {
                    server.Plugins.ParseItems(null, luaScope, parameters);
                }
                else if (nodeType == "monsters")
                {
                    server.Plugins.ParseMonsters(null, luaScope, parameters);
                }
                else if (nodeType == "npcs")
                {
                    server.Plugins.ParseNpcs(null, luaScope, parameters);
                }
                else if (nodeType == "players")
                {
                    server.Plugins.ParsePlayers(null, luaScope, parameters);
                }
                else if (nodeType == "spells")
                {
                    server.Plugins.ParseSpells(null, luaScope, parameters);
                }
                else if (nodeType == "runes")
                {
                    server.Plugins.ParseRunes(null, luaScope, parameters);
                }
                else if (nodeType == "weapons")
                {
                    server.Plugins.ParseWeapons(null, luaScope, parameters);
                }
                else if (nodeType == "ammunitions")
                {
                    server.Plugins.ParseAmmunitions(null, luaScope, parameters);
                }
                else if (nodeType == "raids")
                {
                    server.Plugins.ParseRaids(null, luaScope, parameters);
                }
                else if (nodeType == "monsterattacks")
                {
                    server.Plugins.ParseMonsterAttacks(null, luaScope, parameters);
                }

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("waithandle", (luaScope, args) =>
            {
                var promise = new PromiseResult<object[]>();

                return Promise.FromResult(new object[] { promise } );
            } );

            lua.RegisterCoFunction("wait", (luaScope, args) =>
            {
                var promise = (PromiseResult<object[]>)args[0];

                return promise.Then(result =>
                {
                    return Promise.FromResult(result);
                } ); 
            } );

            lua.RegisterCoFunction("set", (luaScope, args) =>
            {
                var promise = (PromiseResult<object[]>)args[0];

                promise.TrySetResult(args.Skip(1).ToArray() );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("yield", (luaScope, args) =>
            {
                _ = Promise.Yield().Then( () =>
                {                          
                    return luaScope.CallFunction( (LuaFunction)args[0] ); // Ignore result
                } );
                        
                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("delay", (luaScope, args) =>
            {             
                if (args[0] is GameObject)
                {
                    MultipleDelayBehaviour multipleDelayBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)args[0], new MultipleDelayBehaviour(TimeSpan.FromMilliseconds(LuaScope.GetInt64(args[1] ) ) ), false);

                    if (args.Length == 3)
                    {
                        // string command.delay(GameObject gameObject, int milliseconds, Action callback)

                        _ = multipleDelayBehaviour.Promise.Then( () =>
                        {
                            return luaScope.CallFunction( (LuaFunction)args[2] ); // Ignore result
                        } );

                        return Promise.FromResult(new object[] { multipleDelayBehaviour.Key } );
                    }

                    // void command.delay(GameObject gameObject, int milliseconds) block

                    return multipleDelayBehaviour.Promise.Then( () =>
                    {
                        return Promise.FromResultAsEmptyObjectArray;
                    } );
                }
                else
                {
                    string key = Guid.NewGuid().ToString();

                    Promise promise = Promise.Delay(key, TimeSpan.FromMilliseconds(LuaScope.GetInt64(args[0] ) ) );

                    if (args.Length == 2)
                    {
                        // string command.delay(int milliseconds, Action callback)

                        _ = promise.Then( () =>
                        {
                            return luaScope.CallFunction( (LuaFunction)args[1] ); // Ignore result
                        } ); 

                        return Promise.FromResult(new object[] { key } );
                    }

                    // void command.delay(int milliseconds) block

                    return promise.Then( () =>
                    {
                        return Promise.FromResultAsEmptyObjectArray;
                    } );
                }                                
            } );

            lua.RegisterCoFunction("canceldelay", (luaScope, args) =>
            {
                bool canceled = Context.Current.Server.CancelQueueForExecution(LuaScope.GetString(args[0] ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("eventhandler", (luaScope, args) =>
            {
                if (args[0] is GameObject)
                {
                    // string command.eventhandler(GameObject gameObject, string eventName, Action<GameEventArgs> callback)

                    MultipleEventHandlerBehaviour multipleEventHandlerBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)args[0], new MultipleEventHandlerBehaviour(Type.GetType(LuaScope.GetString(args[1] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)args[2], e); // Ignore result

                    } ), false);

                    return Promise.FromResult(new object[] { multipleEventHandlerBehaviour.Key.ToString() } );
                }
                else
                {
                    // string command.eventhandler(string eventName, Action<GameEventArgs> callback)

                    Guid key = Context.Current.Server.EventHandlers.Subscribe(Type.GetType(LuaScope.GetString(args[0] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)args[1], e); // Ignore result
                    } );

                    return Promise.FromResult(new object[] { key.ToString() } );
                }
            } );

            lua.RegisterCoFunction("canceleventhandler", (luaScope, args) =>
            {
                bool canceled = Context.Current.Server.EventHandlers.Unsubscribe(Guid.Parse(LuaScope.GetString(args[0] ) ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("gameobjecteventhandler", (luaScope, args) =>
            {
                if (args[0] is GameObject)
                {
                    // string command.gameobjecteventhandler(GameObject gameObject, GameObject eventSource, string eventName, Action<GameEventArgs> callback)

                    MultipleGameObjectEventHandlerBehaviour multipleEventHandlerBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)args[0], new MultipleGameObjectEventHandlerBehaviour( (GameObject)args[1], Type.GetType(LuaScope.GetString(args[2] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)args[3], e); // Ignore result

                    } ), false);

                    return Promise.FromResult(new object[] { multipleEventHandlerBehaviour.Key.ToString() } );
                }
                else
                {
                    // string command.gameobjecteventhandler(GameObject eventSource, string eventName, Action<GameEventArgs> callback)

                    Guid key = Context.Current.Server.GameObjectEventHandlers.Subscribe( (GameObject)args[0], Type.GetType(LuaScope.GetString(args[1] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)args[2], e); // Ignore result
                    } );

                    return Promise.FromResult(new object[] { key.ToString() } );
                }
            } );

            lua.RegisterCoFunction("cancelgameobjecteventhandler", (luaScope, args) =>
            {
                bool canceled = Context.Current.Server.GameObjectEventHandlers.Unsubscribe(Guid.Parse(LuaScope.GetString(args[0] ) ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("positionaleventhandler", (luaScope, args) =>
            {
                if (args[0] is GameObject)
                {
                    // string command.positionaleventhandler(GameObject gameObject, GameObject observer, string eventName, Action<GameEventArgs> callback)

                    MultiplePositionalEventHandlerBehaviour multipleEventHandlerBehaviour = Context.Current.Server.GameObjectComponents.AddComponent( (GameObject)args[0], new MultiplePositionalEventHandlerBehaviour( (GameObject)args[1], Type.GetType(LuaScope.GetString(args[2] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)args[3], e); // Ignore result

                    } ), false);

                    return Promise.FromResult(new object[] { multipleEventHandlerBehaviour.Key.ToString() } );
                }
                else
                {
                    // string command.positionaleventhandler(GameObject observer, string eventName, Action<GameEventArgs> callback)

                    Guid key = Context.Current.Server.PositionalEventHandlers.Subscribe( (GameObject)args[0], Type.GetType(LuaScope.GetString(args[1] ) ), (context, e) =>
                    {
                        return luaScope.CallFunction( (LuaFunction)args[2], e); // Ignore result
                    } );

                    return Promise.FromResult(new object[] { key.ToString() } );
                }
            } );

            lua.RegisterCoFunction("cancelpositionaleventhandler", (luaScope, args) =>
            {
                bool canceled = Context.Current.Server.PositionalEventHandlers.Unsubscribe(Guid.Parse(LuaScope.GetString(args[0] ) ) );

                return canceled ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("containeradditem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ContainerAddItemCommand( (Container)args[0], (Item)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("containercreateitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ContainerCreateItemCommand( (Container)args[0], LuaScope.GetUInt16(args[1] ), LuaScope.GetByte(args[2] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("containerremoveitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ContainerRemoveItemCommand( (Container)args[0], (Item)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("containerreplaceitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ContainerReplaceItemCommand( (Container)args[0], (Item)args[1], (Item)args[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureaddcondition", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureAddConditionCommand( (Creature)args[0], ToCondition(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureattackarea", (luaScope, args) =>
            {
                if (args.Length == 8)
                {
                    // void command.creatureattackarea(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack, Condition condition)
			
                    return Context.Current.AddCommand(new CreatureAttackAreaCommand( (Creature)args[0], (bool)args[1], ToPosition(args[2] ), ToOffsetArray(args[3] ), (ProjectileType?)(long?)args[4], (MagicEffectType?)(long?)args[5], ToAttack(args[6] ), ToCondition(args[7] ) ) ).Then( () =>
                    {
                        return Promise.FromResultAsEmptyObjectArray;
                    } );
                }

			    // void command.creatureattackarea(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte typeCount, Attack attack, Condition condition)

                return Context.Current.AddCommand(new CreatureAttackAreaCommand( (Creature)args[0], (bool)args[1], ToPosition(args[2] ), ToOffsetArray(args[3] ), (ProjectileType?)(long?)args[4], (MagicEffectType?)(long?)args[5], LuaScope.GetUInt16(args[6] ), LuaScope.GetByte(args[7] ), ToAttack(args[8] ), ToCondition(args[9] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureattackcreature", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureAttackCreatureCommand( (Creature)args[0], (Creature)args[1], ToAttack(args[2] ), ToCondition(args[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureremovecondition", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureRemoveConditionCommand( (Creature)args[0], (ConditionSpecialCondition)(long)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creaturedestroy", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureDestroyCommand( (Creature)args[0] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatedirection", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateDirectionCommand( (Creature)args[0], (Direction)(long)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatehealth", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateHealthCommand( (Creature)args[0], LuaScope.GetInt32(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdateinvisible", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateInvisibleCommand( (Creature)args[0], (bool)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatelight", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateLightCommand( (Creature)args[0], ToLight(args[1] ), ToLight(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdateoutfit", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateOutfitCommand( (Creature)args[0], ToOutfit(args[1] ), ToOutfit(args[2] ), LuaScope.GetBoolean(args[3] ), LuaScope.GetBoolean(args[4] ), LuaScope.GetBoolean(args[5] ), LuaScope.GetBoolean(args[6] )  ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creatureupdatespeed", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSpeedCommand( (Creature)args[0], LuaScope.GetInt32(args[1] ), LuaScope.GetInt32(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("creaturemove", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new CreatureMoveCommand( (Creature)args[0], ToTile(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("fluiditemupdatefluidtype", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new FluidItemUpdateFluidTypeCommand( (FluidItem)args[0], (FluidType)(long)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventoryadditem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new InventoryAddItemCommand( (Inventory)args[0], LuaScope.GetByte(args[1] ), (Item)args[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventorycreateitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new InventoryCreateItemCommand( (Inventory)args[0], LuaScope.GetByte(args[1] ), LuaScope.GetUInt16(args[2] ), LuaScope.GetByte(args[3] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("inventoryremoveitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new InventoryRemoveItemCommand( (Inventory)args[0], (Item)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("inventoryreplaceitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new InventoryReplaceItemCommand( (Inventory)args[0], (Item)args[1], (Item)args[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemclone", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ItemCloneCommand( (Item)args[0], (bool)args[1] ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("itemdecrement", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ItemDecrementCommand( (Item)args[0], LuaScope.GetByte(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemdestroy", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ItemDestroyCommand( (Item)args[0] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemmove", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ItemMoveCommand( (Item)args[0], (IContainer)args[1], LuaScope.GetByte(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("itemtransform", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ItemTransformCommand( (Item)args[0], LuaScope.GetUInt16(args[1] ), LuaScope.GetByte(args[2] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("monstersay", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new MonsterSayCommand( (Monster)args[0], LuaScope.GetString(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("monsteryell", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new MonsterYellCommand( (Monster)args[0], LuaScope.GetString(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("npcsay", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new NpcSayCommand( (Npc)args[0], LuaScope.GetString(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                        
            lua.RegisterCoFunction("npcsaytoplayer", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new NpcSayToPlayerCommand( (Npc)args[0], (Player)args[1], LuaScope.GetString(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("npctrade", (luaScope, args) =>
            {
                List<OfferDto> offers = new List<OfferDto>();

                foreach (LuaTable item in ( (LuaTable)args[2] ).Values)
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

                return Context.Current.AddCommand(new NpcTradeCommand( (Npc)args[0], (Player)args[1], offers) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("npcidle", (luaScope, args) =>
            {
                if (server.Config.GameplayPrivateNpcSystem && server.Features.HasFeatureFlag(FeatureFlag.NpcsChannel) )
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>( (Npc)args[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Idle( (Player)args[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                else
                {
                    SingleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<SingleQueueNpcThinkBehaviour>( (Npc)args[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Idle( (Player)args[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                
                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("npcfarewell", (luaScope, args) =>
            {
                if (server.Config.GameplayPrivateNpcSystem && server.Features.HasFeatureFlag(FeatureFlag.NpcsChannel) )
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>( (Npc)args[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Farewell( (Player)args[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                else
                {
                    SingleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<SingleQueueNpcThinkBehaviour>( (Npc)args[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Farewell( (Player)args[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("npcdisappear", (luaScope, args) =>
            {
                if (server.Config.GameplayPrivateNpcSystem && server.Features.HasFeatureFlag(FeatureFlag.NpcsChannel) )
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>( (Npc)args[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Disappear( (Player)args[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }
                else
                {
                    SingleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<SingleQueueNpcThinkBehaviour>( (Npc)args[0] );

                    if (npcThinkBehaviour != null)
                    {
                        return npcThinkBehaviour.Disappear( (Player)args[1] ).Then( () =>
                        {
                            return Promise.FromResultAsEmptyObjectArray;
                        } );
                    }
                }

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playercreatemoney", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerCreateMoneyCommand( (Player)args[0], LuaScope.GetInt32(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerdestroymoney", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerDestroyMoneyCommand( (Player)args[0], LuaScope.GetInt32(args[1] ) ) ).Then( (success) =>
                {
                    return success ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("playercountmoney", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerCountMoneyCommand( (Player)args[0] ) ).Then( (price) =>
                {
                    return Promise.FromResult(new object[] { price } );
                } );                  
            } );

            lua.RegisterCoFunction("playercreateitems", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerCreateItemsCommand( (Player)args[0], LuaScope.GetUInt16(args[1] ), LuaScope.GetByte(args[2] ), LuaScope.GetInt32(args[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerdestroyitems", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerDestroyItemsCommand( (Player)args[0], LuaScope.GetUInt16(args[1] ), LuaScope.GetByte(args[2] ), LuaScope.GetInt32(args[3] ) ) ).Then( (success) =>
                {
                    return success ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
                } );                
            } );

            lua.RegisterCoFunction("playercountitems", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerCountItemsCommand( (Player)args[0], LuaScope.GetUInt16(args[1] ), LuaScope.GetByte(args[2] ) ) ).Then( (count) =>
                {
                    return Promise.FromResult(new object[] { count } );
                } );                
            } );

            lua.RegisterCoFunction("playerachievement", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerAchievementCommand( (Player)args[0], LuaScope.GetInt32(args[1] ), LuaScope.GetInt32(args[2] ), LuaScope.GetString(args[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerbless", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerBlessCommand( (Player)args[0], LuaScope.GetString(args[1] ), LuaScope.GetString(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playeraddexperience", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerAddExperienceCommand( (Player)args[0], LuaScope.GetUInt64(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerremoveexperience", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerRemoveExperienceCommand( (Player)args[0], LuaScope.GetUInt64(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playeraddskillpoints", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerAddSkillPointsCommand( (Player)args[0], (Skill)(long)args[1], LuaScope.GetUInt64(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerremoveskillpoints", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerRemoveSkillPointsCommand( (Player)args[0], (Skill)(long)args[1], LuaScope.GetUInt64(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateskill", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateSkillCommand( (Player)args[0], (Skill)(long)args[1], LuaScope.GetUInt64(args[2] ), LuaScope.GetByte(args[3] ), LuaScope.GetByte(args[4] ), LuaScope.GetInt32(args[5] ), LuaScope.GetInt32(args[6] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                        
            lua.RegisterCoFunction("playersay", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerSayCommand( (Player)args[0], LuaScope.GetString(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playerupdatecapacity", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateCapacityCommand( (Player)args[0], LuaScope.GetInt32(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdateexperience", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateExperienceCommand( (Player)args[0], LuaScope.GetUInt64(args[1] ), LuaScope.GetUInt16(args[2] ), LuaScope.GetByte(args[3] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatemana", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateManaCommand( (Player)args[0], LuaScope.GetInt32(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatesoul", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateSoulCommand( (Player)args[0], LuaScope.GetInt32(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerupdatestamina", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerUpdateStaminaCommand( (Player)args[0], LuaScope.GetInt32(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("playerwhisper", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerWhisperCommand( (Player)args[0], LuaScope.GetString(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playeryell", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new PlayerYellCommand( (Player)args[0], LuaScope.GetString(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
            
            lua.RegisterCoFunction("playergetoutfit", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                Addon addon;

                if (player.Outfits.TryGetOutfit(LuaScope.GetUInt16(args[1] ), out addon) )
                {
                    return Promise.FromResult(new object[] { true, addon } );
                }

                return Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetoutfit", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Outfits.SetOutfit(LuaScope.GetUInt16(args[1] ), (Addon)(long)args[2] );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremoveoutfit", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Outfits.RemoveOutfit(LuaScope.GetUInt16(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetmounts", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                ushort[] mounts = player.Mounts.GetMounts().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(mounts) } );
            } );

            lua.RegisterCoFunction("playerhasmount", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                return player.Mounts.HasMount(LuaScope.GetUInt16(args[1] ) ) ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetmount", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Mounts.SetMount(LuaScope.GetUInt16(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovemount", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Mounts.RemoveMount(LuaScope.GetUInt16(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetstorage", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                int value;

                if (player.Storages.TryGetValue(LuaScope.GetInt32(args[1] ), out value) )
                {
                    return Promise.FromResult(new object[] { true, value } );
                }

                return Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetstorage", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Storages.SetValue(LuaScope.GetInt32(args[1] ), LuaScope.GetInt32(args[2] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovestorage", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Storages.RemoveValue(LuaScope.GetInt32(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );
            
            lua.RegisterCoFunction("playergetachievements", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                string[] achievements = player.Achievements.GetAchievements().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(achievements) } );
            } );

            lua.RegisterCoFunction("playerhasachievement", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                return player.Achievements.HasAchievement(LuaScope.GetString(args[1] ) ) ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetachievement", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Achievements.SetAchievement(LuaScope.GetString(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremoveachievement", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Achievements.RemoveAchievement(LuaScope.GetString(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetspells", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                string[] spells = player.Spells.GetSpells().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(spells) } );
            } );

            lua.RegisterCoFunction("playerhasspell", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                return player.Spells.HasSpell(LuaScope.GetString(args[1] ) ) ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetspell", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Spells.SetSpell(LuaScope.GetString(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovespell", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Spells.RemoveSpell(LuaScope.GetString(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playergetblesses", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                string[] blesses = player.Blesses.GetBlesses().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(blesses) } );
            } );

            lua.RegisterCoFunction("playerhasbless", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                return player.Blesses.HasBless(LuaScope.GetString(args[1] ) ) ? Promise.FromResultAsBooleanTrueObjectArray : Promise.FromResultAsBooleanFalseObjectArray;
            } );

            lua.RegisterCoFunction("playersetbless", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Blesses.SetBless(LuaScope.GetString(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerremovebless", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                player.Blesses.RemoveBless(LuaScope.GetString(args[1] ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("playerstopwalk", (luaScope, args) =>
            {
                Player player = (Player)args[0];

                Context.Current.AddPacket(player, new StopWalkOutgoingPacket(player.Direction) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("showanimatedtext", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ShowAnimatedTextCommand(ToPosition(args[0] ), (AnimatedTextColor)(long)args[1], LuaScope.GetString(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showmagiceffect", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ShowMagicEffectCommand(ToPosition(args[0] ), (MagicEffectType)(long)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showprojectile", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ShowProjectileCommand(ToPosition(args[0] ), ToPosition(args[1] ), (ProjectileType)(long)args[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showtext", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new ShowTextCommand( (Creature)args[0], (TalkType)(long)args[1], LuaScope.GetString(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("showwindowtext", (luaScope, args) =>
            {
                Context.Current.AddPacket( (Player)args[0], new ShowWindowTextOutgoingPacket( (TextColor)(long)args[1], LuaScope.GetString(args[2] ) ) );

                return Promise.FromResultAsEmptyObjectArray;
            } );

            lua.RegisterCoFunction("splashitemupdatefluidtype", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new SplashItemUpdateFluidTypeCommand( (SplashItem)args[0], (FluidType)(long)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );
                                               
            lua.RegisterCoFunction("stackableitemupdatecount", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new StackableItemUpdateCountCommand( (StackableItem)args[0], LuaScope.GetByte(args[1] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileaddcreature", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileAddCreatureCommand( (Tile)args[0], (Creature)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileadditem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileAddItemCommand( (Tile)args[0], (Item)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilecreateitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileCreateItemCommand( (Tile)args[0], LuaScope.GetUInt16(args[1] ), LuaScope.GetByte(args[2] ) ) ).Then( (item) =>
                {
                    return Promise.FromResult(new object[] { item } );
                } );
            } );

            lua.RegisterCoFunction("tilecreateitemorincrement", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)args[0], LuaScope.GetUInt16(args[1] ), LuaScope.GetByte(args[2] ) ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilecreatemonster", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileCreateMonsterCommand( (Tile)args[0], LuaScope.GetString(args[1] ) ) ).Then( (monster) =>
                {
                    return Promise.FromResult(new object[] { monster } );
                } );
            } );

            lua.RegisterCoFunction("tilecreatenpc", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileCreateNpcCommand( (Tile)args[0], LuaScope.GetString(args[1] ) ) ).Then( (npc) =>
                {
                    return Promise.FromResult(new object[] { npc } );
                } );
            } );

            lua.RegisterCoFunction("tileremovecreature", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileRemoveCreatureCommand ( (Tile)args[0], (Creature)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tileremoveitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileRemoveItemCommand ( (Tile)args[0], (Item)args[1] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("tilereplaceitem", (luaScope, args) =>
            {
                return Context.Current.AddCommand(new TileReplaceItemCommand( (Tile)args[0], (Item)args[1], (Item)args[2] ) ).Then( () =>
                {
                    return Promise.FromResultAsEmptyObjectArray;
                } );
            } );

            lua.RegisterCoFunction("mapgettownbyname", (luaScope, args) =>
            {
                Town town = Context.Current.Server.Map.GetTown(LuaScope.GetString(args[0] ) );

                return Promise.FromResult(new object[] { town } );
            } );

            lua.RegisterCoFunction("mapgetwaypointbyname", (luaScope, args) =>
            {
                Waypoint waypoint = Context.Current.Server.Map.GetWaypoint(LuaScope.GetString(args[0] ) );

                return Promise.FromResult(new object[] { waypoint } );
            } );

            lua.RegisterCoFunction("mapgethousebyname", (luaScope, args) =>
            {
                House house = Context.Current.Server.Map.GetHouse(LuaScope.GetString(args[0] ) );

                return Promise.FromResult(new object[] { house } );
            } );

            lua.RegisterCoFunction("mapgettile", (luaScope, args) =>
            {
                Tile tile = Context.Current.Server.Map.GetTile(ToPosition(args[0] ) );

                return Promise.FromResult(new object[] { tile } );
            } );

            lua.RegisterCoFunction("mapgetobserversoftypeplayer", (luaScope, args) =>
            {
                Player[] players = Context.Current.Server.Map.GetObserversOfTypePlayer(ToPosition(args[0] ) ).ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(players) } );
            } );

            lua.RegisterCoFunction("gameobjectsgetplayers", (luaScope, args) =>
            {
                Player[] players = Context.Current.Server.GameObjects.GetPlayers().ToArray();

                return Promise.FromResult(new object[] { lua.ToTable(players) } );
            } );

            lua.RegisterCoFunction("gameobjectsgetplayerbyname", (luaScope, args) =>
            {
                Player player = Context.Current.Server.GameObjects.GetPlayerByName(LuaScope.GetString(args[0] ) );

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
            if (file == "channels")
            {
                return server.Channels.GetValue(key);
            }
            else if (file == "values")
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

                if (table["tibiaid"] != null)
                {
                    return new Outfit(LuaScope.GetUInt16(table["tibiaid"] ) );
                }

                return new Outfit(LuaScope.GetUInt16(table["id"] ), LuaScope.GetByte(table["head"] ), LuaScope.GetByte(table["body"] ), LuaScope.GetByte(table["legs"] ), LuaScope.GetByte(table["feet"] ), (Addon)(long)table["addon"], LuaScope.GetUInt16(table["mount"] ) );
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

                        return new DamageAttack( (ProjectileType?)(long?)table["projectiletype"], (MagicEffectType?)(long?)table["magiceffecttype"], (DamageType)(long)table["damagetype"], LuaScope.GetInt32(table["min"] ), LuaScope.GetInt32(table["max"] ), LuaScope.GetBoolean(table["blockable"] ) );
                    
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

                        return new HasteCondition(LuaScope.GetUInt16(table["conditionspeed"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "stealth":

                        return new StealthCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "light":

                        return new LightCondition(ToLight(table["conditionlight"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "logoutblock":

                        return new LogoutBlockCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "magicshield":

                        return new MagicShieldCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "outfit":

                        return new OutfitCondition(ToOutfit(table["conditionoutfit"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );
                               
                    case "protectionzoneblock":

                        return new ProtectionZoneBlockCondition(TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );

                    case "slowed":

                        return new SlowedCondition(LuaScope.GetUInt16(table["conditionspeed"] ), TimeSpan.FromSeconds(LuaScope.GetInt32(table["duration"] ) ) );
                }
            }
         
            throw new ArgumentException("Parameter must be Condition or LuaTable with type, specialcondition, magiceffecttype, animatedtextcolor, damages, interval, damage, conditionspeed, duration, conditionlight and/or conditionoutfit.");
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