﻿using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Items
{
    public class Item
    {
        public static Item Load(XElement itemNode)
        {
            Item item = new Item();

            item.OpenTibiaId = (ushort)(uint)itemNode.Attribute("id");

            item.Article = (string)itemNode.Attribute("article");

            item.Name = (string)itemNode.Attribute("name");

            item.Plural = (string)itemNode.Attribute("plural");

            item.EditorSuffix = (string)itemNode.Attribute("editorsuffix");

            item.EditorCategory = (string)itemNode.Attribute("editorcategory");

            item.Plural = (string)itemNode.Attribute("plural");

            foreach (var attributeNode in itemNode.Elements("attribute") )
            {
                XAttribute key = attributeNode.Attribute("key");

                XAttribute value = attributeNode.Attribute("value");

                switch ( (string)key )
                {
                    case "article":

                        item.Article = (string)value;

                        break;

                    case "name":

                        item.Name = (string)value;

                        break;

                    case "plural":

                        item.Plural = (string)value;

                        break;

                    case "editorsuffix":

                        item.EditorSuffix = (string)value;

                        break;

                    case "editorcategory":

                        item.EditorCategory = (string)value;

                        break;

                    case "description":

                        item.Description = (string)value;

                        break;

                    case "runespellname":

                        item.RuneSpellName = (string)value;

                        break;

                    case "weight":

                        item.Weight = (uint)value;

                        break;

                    case "armor":

                        item.Armor = (byte)(uint)value;

                        break;

                    case "defense":

                        item.Defense = (byte)(uint)value;

                        break;

                    case "defensemodifier":

                        item.DefenseModifier = (int)value;

                        break;

                    case "attack":

                        item.Attack = (byte)(uint)value;

                        break;

                    case "blockprojectile":

                        item.BlockProjectile = true;

                        break;

                    case "floorchange":

                        FloorChange floorChange = Common.Structures.FloorChange.None;

                        switch ( (string)value )
	                    {
		                    case "down":

                                floorChange |= Common.Structures.FloorChange.Down;

                                break;

                            case "north":

                                floorChange |= Common.Structures.FloorChange.North;

                                break;

                            case "south":

                                floorChange |= Common.Structures.FloorChange.South;

                                break;

                            case "west":

                                floorChange |= Common.Structures.FloorChange.West;

                                break;

                            case "east":

                                floorChange |= Common.Structures.FloorChange.East;

                                break;

                            case "northeast":

                                floorChange |= Common.Structures.FloorChange.NorthEast;

                                break;

                            case "northwest":

                                floorChange |= Common.Structures.FloorChange.NorthWest;

                                break;

                            case "southwest":

                                floorChange |= Common.Structures.FloorChange.SouthWest;

                                break;

                            case "southeast":

                                floorChange |= Common.Structures.FloorChange.SouthEast;

                                break;
                        }

                        if (item.FloorChange == null)
                        {
                            item.FloorChange = floorChange;
                        }
                        else
                        {
                            item.FloorChange |= floorChange;
                        }

                        break;

                    case "corpsetype":

                        switch ( (string)value)
                        {
                            case "blood":

                                item.Race = Common.Structures.Race.Blood;

                                break;

                            case "energy":

                                item.Race = Common.Structures.Race.Energy;

                                break;

                            case "fire":

                                item.Race = Common.Structures.Race.Fire;

                                break;

                            case "venom":

                                item.Race = Common.Structures.Race.Venom;

                                break;

                            case "undead":

                                item.Race = Common.Structures.Race.Undead;

                                break;
                        }

                        break;

                    case "containersize":

                        item.ContainerSize = (byte)(uint)value;

                        break;

                    case "weapontype":

                        switch ( (string)value)
                        {
                            case "sword":

                                item.WeaponType = Common.Structures.WeaponType.Sword;

                                break;

                            case "club":

                                item.WeaponType = Common.Structures.WeaponType.Club;

                                break;

                            case "axe":

                                item.WeaponType = Common.Structures.WeaponType.Axe;

                                break;

                            case "shield":

                                item.WeaponType = Common.Structures.WeaponType.Shield;

                                break;

                            case "distance":

                                item.WeaponType = Common.Structures.WeaponType.Distance;

                                break;

                            case "wand":
                            case "rod":

                                item.WeaponType = Common.Structures.WeaponType.Wand;

                                break;

                            case "ammunition":

                                item.WeaponType = Common.Structures.WeaponType.Ammunition;

                                break;
                        }

                        break;

                    case "ammotype":

                        switch ( (string)value)
                        {
                            case "arrow":

                                item.AmmoType = Common.Structures.AmmoType.Arrow;

                                break;

                            case "bolt":

                                item.AmmoType = Common.Structures.AmmoType.Bolt;

                                break;
                        }

                        break;

                    case "shoottype":

                        switch ( (string)value)
                        {
                            case "spear":

                                item.ProjectileType = Common.Structures.ProjectileType.Spear;

                                break;

                            case "bolt":

                                item.ProjectileType = Common.Structures.ProjectileType.Bolt;

                                break;

                            case "arrow":

                                item.ProjectileType = Common.Structures.ProjectileType.Arrow;

                                break;

                            case "fire":

                                item.ProjectileType = Common.Structures.ProjectileType.Fire;

                                break;

                            case "energy":

                                item.ProjectileType = Common.Structures.ProjectileType.Energy;

                                break;

                            case "poisonarrow":

                                item.ProjectileType = Common.Structures.ProjectileType.PoisonArrow;

                                break;

                            case "burstarrow":

                                item.ProjectileType = Common.Structures.ProjectileType.BurstArrow;

                                break;

                            case "throwingstar":

                                item.ProjectileType = Common.Structures.ProjectileType.ThrowingStar;

                                break;

                            case "throwingknife":

                                item.ProjectileType = Common.Structures.ProjectileType.ThrowingKnife;

                                break;

                            case "smallstone":

                                item.ProjectileType = Common.Structures.ProjectileType.SmallStone;

                                break;

                            case "death":
                            case "skull":

                                item.ProjectileType = Common.Structures.ProjectileType.Skull;

                                break;

                            case "largerock":
                            case "bigstone":

                                item.ProjectileType = Common.Structures.ProjectileType.BigStone;

                                break;

                            case "snowball":

                                item.ProjectileType = Common.Structures.ProjectileType.Snowball;

                                break;

                            case "powerbolt":

                                item.ProjectileType = Common.Structures.ProjectileType.PowerBolt;

                                break;

                            case "poison":

                                item.ProjectileType = Common.Structures.ProjectileType.Poison;

                                break;

                            case "infernalbolt":

                                item.ProjectileType = Common.Structures.ProjectileType.InfernalBolt;

                                break;

                            case "huntingspear":

                                item.ProjectileType = Common.Structures.ProjectileType.HuntingSpear;

                                break;

                            case "enchantedspear":

                                item.ProjectileType = Common.Structures.ProjectileType.EnchantedSpear;

                                break;

                            case "redstar":
                            case "assassinstar":

                                item.ProjectileType = Common.Structures.ProjectileType.AssassinStar;

                                break;

                            case "greenstar":
                            case "viperstar":

                                item.ProjectileType = Common.Structures.ProjectileType.ViperStar;

                                break;

                            case "royalspear":

                                item.ProjectileType = Common.Structures.ProjectileType.RoyalSpear;

                                break;

                            case "sniperarrow":

                                item.ProjectileType = Common.Structures.ProjectileType.SniperArrow;

                                break;

                            case "onyxarrow":

                                item.ProjectileType = Common.Structures.ProjectileType.OnyxArrow;

                                break;

                            case "piercingbolt":

                                item.ProjectileType = Common.Structures.ProjectileType.PiercingBolt;

                                break;

                            case "whirlwindsword":

                                item.ProjectileType = Common.Structures.ProjectileType.WhirlWindSword;

                                break;

                            case "whirlwindaxe":

                                item.ProjectileType = Common.Structures.ProjectileType.WhirlWindAxe;

                                break;

                            case "whirlwindclub":

                                item.ProjectileType = Common.Structures.ProjectileType.WhirlWindClub;

                                break;

                            case "etherealspear":

                                item.ProjectileType = Common.Structures.ProjectileType.EtherenalSpear;

                                break;

                            case "ice":

                                item.ProjectileType = Common.Structures.ProjectileType.Ice;

                                break;

                            case "earth":

                                item.ProjectileType = Common.Structures.ProjectileType.Earth;

                                break;

                            case "holy":

                                item.ProjectileType = Common.Structures.ProjectileType.Holy;

                                break;

                            case "suddendeath":

                                item.ProjectileType = Common.Structures.ProjectileType.SuddenDeath;

                                break;

                            case "flasharrow":

                                item.ProjectileType = Common.Structures.ProjectileType.FlashArrow;

                                break;

                            case "flamingarrow":

                                item.ProjectileType = Common.Structures.ProjectileType.FlamingArrow;

                                break;

                            case "shiverarrow":

                                item.ProjectileType = Common.Structures.ProjectileType.ShiverArrow;

                                break;

                            case "energyball":
                            case "energysmall":

                                item.ProjectileType = Common.Structures.ProjectileType.EnergySmall;

                                break;

                            case "smallice":
                            case "icesmall":

                                item.ProjectileType = Common.Structures.ProjectileType.IceSmall;

                                break;

                            case "smallholy":
                            case "holysmall":

                                item.ProjectileType = Common.Structures.ProjectileType.HolySmall;

                                break;

                            case "smallearth":
                            case "earthsmall":

                                item.ProjectileType = Common.Structures.ProjectileType.EarthSmall;

                                break;

                            case "eartharrow":

                                item.ProjectileType = Common.Structures.ProjectileType.EarthArrow;

                                break;

                            case "explosion":

                                item.ProjectileType = Common.Structures.ProjectileType.Explosion;

                                break;

                            case "cake":

                                item.ProjectileType = Common.Structures.ProjectileType.Cake;

                                break;
                        }

                        break;

                    case "effect":

                        switch ( (string)value)
                        {
                            case "redspark":
                           
                                item.MagicEffectType = Common.Structures.MagicEffectType.RedSpark;
                                
                                break;
                            
                            case "bluebubble":
                            case "bluerings":

                                item.MagicEffectType = Common.Structures.MagicEffectType.BlueRings;
                                
                                break;
                            
                            case "poff":
                            case "puff":

                                item.MagicEffectType = Common.Structures.MagicEffectType.Puff;
                                
                                break;
                            
                            case "yellowspark":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.YellowSpark;
                                
                                break;
                            
                            case "explosionarea":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.ExplosionArea;
                                
                                break;
                            
                            case "explosion":
                            case "explosiondamage":

                                item.MagicEffectType = Common.Structures.MagicEffectType.ExplosionDamage;
                                
                                break;
                            
                            case "firearea":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.FireArea;
                                
                                break;
                            
                            case "yellowbubble":
                            case "yellowrings":

                                item.MagicEffectType = Common.Structures.MagicEffectType.YellowRings;
                                
                                break;
                            
                            case "greenbubble":
                            case "greenrings":

                                item.MagicEffectType = Common.Structures.MagicEffectType.GreenRings;

                                break;

                            case "blackspark":

                                item.MagicEffectType = Common.Structures.MagicEffectType.BlackSpark;

                                break;
                            
                            case "teleport":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Teleport;
                                
                                break;
                            
                            case "energy":
                            case "energydamage":

                                item.MagicEffectType = Common.Structures.MagicEffectType.EnergyDamage;
                                
                                break;
                            
                            case "blueshimmer":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.BlueShimmer;
                                
                                break;
                            
                            case "redshimmer":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.RedShimmer;
                                
                                break;
                            
                            case "greenshimmer":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.GreenShimmer;
                                
                                break;
                            
                            case "fire":
                            case "firedamage":

                                item.MagicEffectType = Common.Structures.MagicEffectType.FireDamage;
                                
                                break;
                          
                            case "greenspark":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.GreenSpark;
                                
                                break;
                         
                            case "mortarea":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.MortArea;
                                
                                break;

                            case "greennote":
                            case "greennotes":

                                item.MagicEffectType = Common.Structures.MagicEffectType.GreenNotes;
                                
                                break;
                           
                            case "rednote":
                            case "rednotes":

                                item.MagicEffectType = Common.Structures.MagicEffectType.RedNotes;
                                
                                break;
                         
                            case "poison":
                            case "poisonarea":

                                item.MagicEffectType = Common.Structures.MagicEffectType.PoisonArea;
                                
                                break;

                            case "yellownote":
                            case "yellownotes":

                                item.MagicEffectType = Common.Structures.MagicEffectType.YellowNotes;
                                
                                break;
                         
                            case "purplenote":
                            case "purplenotes":

                                item.MagicEffectType = Common.Structures.MagicEffectType.PurpleNotes;
                                
                                break;

                            case "bluenote":
                            case "bluenotes":

                                item.MagicEffectType = Common.Structures.MagicEffectType.BlueNotes;
                                
                                break;
                      
                            case "whitenote":
                            case "whitenotes":

                                item.MagicEffectType = Common.Structures.MagicEffectType.WhiteNotes;
                                
                                break;

                            case "bubbles":
                        
                                item.MagicEffectType = Common.Structures.MagicEffectType.Bubbles;
                                
                                break;
                         
                            case "dice":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Dice;
                                
                                break;
                         
                            case "giftwraps":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.GiftWraps;
                                
                                break;
                         
                            case "yellowfirework":
                            case "fireworkyellow":

                                item.MagicEffectType = Common.Structures.MagicEffectType.FireworkYellow;
                                
                                break;
                        
                            case "redfirework":
                            case "fireworkred":

                                item.MagicEffectType = Common.Structures.MagicEffectType.FireworkRed;
                                
                                break;
                        
                            case "bluefirework":
                            case "fireworkblue":

                                item.MagicEffectType = Common.Structures.MagicEffectType.FireworkBlue;
                                
                                break;

                            case "stun":
                        
                                item.MagicEffectType = Common.Structures.MagicEffectType.Stun;
                                
                                break;
                          
                            case "sleep":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Sleep;
                                
                                break;
                         
                            case "watercreature":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.WaterCreature;
                                
                                break;
                        
                            case "groundshaker":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.GroundShaker;
                                
                                break;
                        
                            case "hearts":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Hearts;
                                
                                break;
                          
                            case "fireattack":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.FireAttack;
                                
                                break;
                         
                            case "energyarea":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.EnergyArea;
                                
                                break;
                         
                            case "smallclouds":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.SmallClouds;
                                
                                break;
                         
                            case "holydamage":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.HolyDamage;
                                
                                break;
                        
                            case "bigclouds":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.BigClouds;
                                
                                break;
                        
                            case "icearea":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.IceArea;
                                
                                break;
                        
                            case "icetornado":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.IceTornado;
                                
                                break;
                       
                            case "iceattack":
                            case "icedamage":

                                item.MagicEffectType = Common.Structures.MagicEffectType.IceDamage;
                                
                                break;
                         
                            case "stones":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Stones;
                                
                                break;
                         
                            case "smallplants":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.SmallPlants;
                                
                                break;
                          
                            case "carniphila":

                                item.MagicEffectType = Common.Structures.MagicEffectType.Carniphila;
                                
                                break;
                          
                            case "purpleenergy":
                            case "purpleenergydamage":

                                item.MagicEffectType = Common.Structures.MagicEffectType.PurpleEnergyDamage;
                                
                                break;
                          
                            case "yellowenergy":
                            case "yellowenergydamage":

                                item.MagicEffectType = Common.Structures.MagicEffectType.YellowEnergyDamage;
                                
                                break;
                         
                            case "holyarea":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.HolyArea;
                                
                                break;

                            case "bigplants":
                         
                                item.MagicEffectType = Common.Structures.MagicEffectType.BigPlants;
                                
                                break;
                        
                            case "cake":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Cake;
                                
                                break;
                        
                            case "giantice":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.GiantIce;
                                
                                break;
                       
                            case "watersplash":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.WaterSplash;
                                
                                break;
                        
                            case "plantattack":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.PlantAttack;
                                
                                break;
                         
                            case "tutorialarrow":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.TutorialArrow;
                                
                                break;
                       
                            case "tutorialsquare":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.TutorialSquare;
                                
                                break;
                       
                            case "mirrorhorizontal":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.MirrorHorizontal;
                                
                                break;
                        
                            case "mirrorvertical":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.MirrorVertical;
                                
                                break;
                       
                            case "skullhorizontal":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.SkullHorizontal;
                                
                                break;
                      
                            case "skullvertical":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.SkullVertical;
                                
                                break;
                        
                            case "assassin":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Assassin;
                                
                                break;
                        
                            case "stepshorizontal":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.StepsHorizontal;
                                
                                break;
                        
                            case "bloodysteps":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.BloodySteps;
                                
                                break;
                            case "stepsvertical":
                        
                                item.MagicEffectType = Common.Structures.MagicEffectType.StepsVertical;
                                
                                break;
                         
                            case "yalaharighost":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.YalahariGhost;
                                
                                break;
                       
                            case "bats":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Bats;
                                
                                break;
                        
                            case "smoke":
                            
                                item.MagicEffectType = Common.Structures.MagicEffectType.Smoke;
                                
                                break;
                        
                            case "insects":
                        
                                item.MagicEffectType = Common.Structures.MagicEffectType.Insects;
                        
                                break;
                         
                            case "dragonhead":
                          
                                item.MagicEffectType = Common.Structures.MagicEffectType.DragonHead;
                              
                                break;
                        }

                        break;

                    case "range":

                        item.Range = (byte)(int)value;

                        break;

                    case "charges":

                        item.Charges = (int)value;

                        break;

                    case "slottype":

                        SlotType slotType = Common.Structures.SlotType.None;

                        switch ( (string)value)
                        {
                            case "head":

                                slotType |= Common.Structures.SlotType.Head;

                                break;

                            case "body":

                                slotType |= Common.Structures.SlotType.Body;

                                break;

                            case "legs":

                                slotType |= Common.Structures.SlotType.Legs;

                                break;

                            case "feet":

                                slotType |= Common.Structures.SlotType.Feet;

                                break;

                            case "backpack":

                                slotType |= Common.Structures.SlotType.Backpack;

                                break;

                            case "two-handed":
                            case "twohanded":

                                slotType |= Common.Structures.SlotType.TwoHanded;

                                break;

                            case "necklace":

                                slotType |= Common.Structures.SlotType.Necklace;

                                break;

                            case "ring":

                                slotType |= Common.Structures.SlotType.Ring;

                                break;

                            case "ammo":

                                slotType |= Common.Structures.SlotType.Ammo;

                                break;

                            case "hand":

                                slotType |= Common.Structures.SlotType.Hand;

                                break;
                        }

                        if (item.SlotType == null)
                        {
                            item.SlotType = slotType;
                        }
                        else
                        {
                            item.SlotType |= slotType;
                        }

                        break;

                    case "breakchance":

                        item.BreakChance = (byte)Math.Max(0, Math.Min(100, (int)value) );

                        break;

                    case "ammoaction":

                        switch ( (string)value)
                        {
                            case "removecount":
                            case "removecharge":
                            case "remove":

                                item.AmmoAction = Common.Structures.AmmoAction.Remove;

                                break;

                            case "move":
                            case "moveback":

                                item.AmmoAction = Common.Structures.AmmoAction.Move;

                                break;
                        }

                        break;

                    case "hitchance":

                        item.HitChance = (byte)Math.Max(0, Math.Min(100, (int)value) );

                        break;

                    case "maxhitchance":

                        item.MaxHitChance = (byte)Math.Max(0, Math.Min(100, (int)value) );

                        break;

                    case "readable":

                        item.Readable = true;

                        break;

                    case "writeable":

                        item.Writeable = true;

                        break;
                        
                    case "attackmodifierearth":

                        item.AttackModifierEarth = (int)value;

                        break;

                    case "attackmodifierfire":

                        item.AttackModifierFire = (int)value;

                        break;

                    case "attackmodifierenergy":

                        item.AttackModifierEnergy = (int)value;

                        break;

                    case "attackmodifierice":

                        item.AttackModifierIce = (int)value;

                        break;

                    case "attackmodifierdeath":

                        item.AttackModifierDeath = (int)value;

                        break;

                    case "attackmodifierholy":

                        item.AttackModifierHoly = (int)value;

                        break;

                    case "attackmodifierdrown":

                        item.AttackModifierDrown = (int)value;

                        break;

                    case "attackmodifiermanadrain":

                        item.AttackModifierManaDrain = (int)value;

                        break;

                    case "attackmodifierlifedrain":

                        item.AttackModifierLifeDrain = (int)value;

                        break;

                    case "absorbpercentphysical":

                        item.AbsorbPhysicalPercent = (int)value;

                        break;

                    case "absorbpercentearth":

                        item.AbsorbEarthPercent = (int)value;

                        break;

                    case "absorbpercentfire":

                        item.AbsorbFirePercent = (int)value;

                        break;

                    case "absorbpercentenergy":

                        item.AbsorbEnergyPercent = (int)value;

                        break;

                    case "absorbpercentice":

                        item.AbsorbIcePercent = (int)value;

                        break;

                    case "absorbpercentdeath":

                        item.AbsorbDeathPercent = (int)value;

                        break;

                    case "absorbpercentholy":

                        item.AbsorbHolyPercent = (int)value;

                        break;

                    case "absorbpercentmanadrain":

                        item.AbsorbManaDrainPercent = (int)value;

                        break;

                    case "absorbpercentlifedrain":

                        item.AbsorbLifeDrainPercent = (int)value;

                        break;

                    case "speedmodifier":

                        item.SpeedModifier = (int)value;

                        break;

                    case "skillmodifiermagiclevel":

                        item.SkillModifierMagicLevel = (int)value;

                        break;

                    case "skillmodifierfist":

                        item.SkillModifierFist = (int)value;

                        break;

                    case "skillmodifiersword":

                        item.SkillModifierSword = (int)value;

                        break;

                    case "skillmodifieraxe":

                        item.SkillModifierAxe = (int)value;

                        break;

                    case "skillmodifierclub":

                        item.SkillModifierClub = (int)value;

                        break;

                    case "skillmodifierdistance":

                        item.SkillModifierDistance = (int)value;

                        break;

                    case "skillmodifiershield":

                        item.SkillModifierShield = (int)value;

                        break;

                    case "skillmodifierfish":

                        item.SkillModifierFish = (int)value;

                        break;

                    default:

                        if (item.Unused == null)
                        {
                            item.Unused = new Dictionary<string, string>();
                        }

                        item.Unused[key.Value] = value.Value;

                        break;
                }
            }

            return item;
        }

        public ushort OpenTibiaId { get; set; }

        public string Article { get; set; }

        public string Name { get; set; }

        public string Plural { get; set; }

        public string EditorSuffix { get; set; }

        public string EditorCategory { get; set; }

        public string Description { get; set; }

        public string RuneSpellName { get; set; }

        public uint? Weight { get; set; }

        public byte? Armor { get; set; }

        public byte? Defense { get; set; }

        public int? DefenseModifier { get; set; }

        public byte? Attack { get; set; }

        public int? AttackModifierEarth { get; set; }

        public int? AttackModifierFire { get; set; }

        public int? AttackModifierEnergy { get; set; }

        public int? AttackModifierIce { get; set; }

        public int? AttackModifierDeath { get; set; }

        public int? AttackModifierHoly { get; set; }

        public int? AttackModifierDrown { get; set; }

        public int? AttackModifierManaDrain { get; set; }

        public int? AttackModifierLifeDrain { get; set; }

        public bool? BlockProjectile { get; set; }

        public Race? Race { get; set; }

        public FloorChange? FloorChange { get; set; }

        public byte? ContainerSize { get; set; }

        public WeaponType? WeaponType { get; set; }

        public AmmoType? AmmoType { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public byte? Range { get; set; }

        public int? Charges { get; set; }

        public SlotType? SlotType { get; set; }

        public byte? BreakChance { get; set; }

        public AmmoAction? AmmoAction { get; set; }

        public byte? HitChance { get; set; }

        public byte? MaxHitChance { get; set; }

        public bool? Readable { get; set; }

        public bool? Writeable { get; set; }

        public int? AbsorbPhysicalPercent { get; set; }

        public int? AbsorbEarthPercent { get; set; }

        public int? AbsorbFirePercent { get; set; }

        public int? AbsorbEnergyPercent { get; set; }

        public int? AbsorbIcePercent { get; set; }

        public int? AbsorbDeathPercent { get; set; }

        public int? AbsorbHolyPercent { get; set; }

        public int? AbsorbDrownPercent { get; set; }

        public int? AbsorbManaDrainPercent { get; set; }

        public int? AbsorbLifeDrainPercent { get; set; }

        public int? SpeedModifier { get; set; }

        public int? SkillModifierMagicLevel { get; set; }

        public int? SkillModifierFist { get; set; }

        public int? SkillModifierSword { get; set; }

        public int? SkillModifierAxe { get; set; }

        public int? SkillModifierClub { get; set; }

        public int? SkillModifierDistance { get; set; }

        public int? SkillModifierShield { get; set; }

        public int? SkillModifierFish { get; set; }

        public Dictionary<string, string> Unused { get; set; }
    }
}