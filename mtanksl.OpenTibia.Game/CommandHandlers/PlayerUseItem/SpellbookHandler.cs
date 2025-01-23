using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpellbookHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> spellbooks;

        public SpellbookHandler()
        {
            spellbooks = Context.Server.Values.GetUInt16HashSet("values.items.spellbooks");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (spellbooks.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                {
                    command.Player.Client.Windows.CloseWindow(pair.Key);
                }

                Window window = new Window();

                window.Item = command.Item;

                uint windowId = command.Player.Client.Windows.OpenWindow(window);

                IEnumerable<Spell> spells = Context.Server.Plugins.Spells;

                if (command.Player.Rank != Rank.Gamemaster)
                {
                    spells = spells.Where(s => s.Vocations.Contains(command.Player.Vocation) );

                    spells = spells.Where(s => s.Level <= command.Player.Level);
                
                    if (Context.Server.Config.GameplayLearnSpellFirst)
                    {
                        spells = spells.Where(s => command.Player.Spells.HasSpell(s.Name) );
                    }

                    if ( !command.Player.Premium)
                    {
                        spells = spells.Where(s => !s.Premium);
                    }
                }

                StringBuilder builder = new StringBuilder();

                foreach (var group in spells.OrderBy(s => s.Level).ThenBy(s => s.Mana).GroupBy(s => s.Level) )
                {
                    builder.Append(group.Key + ". Level Spells\n");

                    foreach (var spell in group)
                    {
                        builder.Append(" " + spell.Words + " - " + spell.Name + ": " + spell.Mana + "\n");
                    }

                    builder.Append("\n");
                }

                if (builder.Length > 2)
                {
                    builder.Remove(builder.Length - 2, 2);
                }

                Context.AddPacket(command.Player, new OpenShowOrEditTextDialogOutgoingPacket(windowId, command.Item.Metadata.TibiaId, (ushort)builder.Length, builder.ToString(), null, null) );

                return Promise.Completed;
            }

            return next();
        }
    }
}