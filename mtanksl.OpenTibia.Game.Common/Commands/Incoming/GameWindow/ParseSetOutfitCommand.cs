﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseSetOutfitCommand : IncomingCommand
    {
        public ParseSetOutfitCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            if ( !Context.Server.Config.GameplayAllowChangeOutfit)
            {
                return Promise.Break;
            }

            List<OutfitDto> outfits = new List<OutfitDto>();

            foreach (var outfitConfig in Context.Server.Outfits.GetOutfits() )
            {
                if (outfitConfig.Premium && !Player.Premium)
                {
                    continue;
                }

                if (outfitConfig.AvailableAtOnce)
                {
                    Addon addon;

                    if (Player.Outfits.TryGetOutfit(outfitConfig.Id, out addon) )
                    {
                        outfits.Add(new OutfitDto(outfitConfig.Id, outfitConfig.Name, Player.Premium ? addon : Addon.None) );
                    }
                    else
                    {
                        outfits.Add(new OutfitDto(outfitConfig.Id, outfitConfig.Name, Addon.None) );
                    }
                }
                else
                {
                    Addon addon;

                    if (Player.Outfits.TryGetOutfit(outfitConfig.Id, out addon) )
                    {
                        outfits.Add(new OutfitDto(outfitConfig.Id, outfitConfig.Name, Player.Premium ? addon : Addon.None) );
                    }
                }
            }
            
            if (Player.Rank == Rank.Gamemaster)
            {
                outfits.Add(new OutfitDto(Outfit.GamemasterBlue.Id, "Gamemaster", Addon.None) );

                outfits.Add(new OutfitDto(Outfit.GamemasterRed.Id, "Customer Support", Addon.None) );

                outfits.Add(new OutfitDto(Outfit.GamemasterGreen.Id, "Community Manager", Addon.None) );
            }

            Context.AddPacket(Player, new OpenSelectOutfitDialogOutgoingPacket(Player.BaseOutfit, outfits) );

            return Promise.Completed;
        }
    }
}