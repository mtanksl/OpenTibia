using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Text;

namespace OpenTibia.Plugins.Spells
{
    public class FindPersonSpellPlugin : SpellPlugin
    {
        public FindPersonSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            int deltaZ = target.Tile.Position.Z - player.Tile.Position.Z;

            int deltaY = Math.Abs(target.Tile.Position.Y - player.Tile.Position.Y);

            int deltaX = Math.Abs(target.Tile.Position.X - player.Tile.Position.X);

            StringBuilder builder = new StringBuilder();

            if (deltaX <= 4 && deltaY <= 4)
            {
                if (deltaZ < 0)
                {
                    builder.Append(target.Name + " is above you.");
                }
                else if (deltaZ > 0)
                {
                    builder.Append(target.Name + " is below to you.");
                }
                else
                {
                    builder.Append(target.Name + " is standing next to you.");
                }
            }
            else
            {
                if (deltaX <= 100 && deltaY <= 100)
                {
                    if (deltaZ < 0)
                    {
                        builder.Append(target.Name + " is on a higher level to the "); 
                    }
                    else if (deltaZ > 0)
                    {
                        builder.Append(target.Name + " is on a lower level to the ");
                    }
                    else
                    {
                        builder.Append(target.Name + " is to the ");
                    }                    
                }
                else if (deltaX <= 250 && deltaY <= 250)
                {
                    builder.Append(target.Name + " is far to the ");
                }
                else
                {
                    builder.Append(target.Name + " is very far to the ");
                }

                MoveDirection? moveDirection = player.Tile.Position.ToMoveDirection(target.Tile.Position);

                switch (moveDirection.Value)
                {
                    case MoveDirection.East:      builder.Append("east.");       break;
                    case MoveDirection.NorthEast: builder.Append("north-east."); break;
                    case MoveDirection.North:     builder.Append("north.");      break;
                    case MoveDirection.NorthWest: builder.Append("north-west."); break;
                    case MoveDirection.West:      builder.Append("west.");       break;
                    case MoveDirection.SouthWest: builder.Append("south-west."); break;
                    case MoveDirection.South:     builder.Append("south.");      break;
                    case MoveDirection.SouthEast: builder.Append("south-east."); break;
                }
            }

            Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}