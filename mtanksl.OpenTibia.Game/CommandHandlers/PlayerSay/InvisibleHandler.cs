using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InvisibleHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/ghost") )
            {
                if ( !command.Player.Invisible)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureUpdateInvisibleCommand(command.Player, true) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureUpdateOutfitCommand(command.Player, command.Player.BaseOutfit, Outfit.Invisible) );
                    } );
                }
                else
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Teleport) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureUpdateInvisibleCommand(command.Player, false) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureUpdateOutfitCommand(command.Player, command.Player.BaseOutfit, command.Player.BaseOutfit) );
                    } );
                }
            }

            return next();
        }
    }
}