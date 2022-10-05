using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class OpenDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 5099, 5100 },
            { 5101, 5102 },
            { 5108, 5109 },
            { 5110, 5111 },

            { 1210, 1211 },
            { 1213, 1214 },
            { 1219, 1220 },
            { 1221, 1222 },
            { 5138, 5139 },
            { 5141, 5142 },

            { 1232, 1233 },
            { 1235, 1236 },
            { 1237, 1238 },
            { 1239, 1240 },

            { 1250, 1251 },
            { 1253, 1254 },
            { 5515, 5516 },
            { 5517, 5518 },

            { 5117, 5118 },
            { 5119, 5120 },
            { 5126, 5127 },
            { 5128, 5129 },
            { 5135, 5136 },
            { 5144, 5145 },

            //...
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerUseItemCommand command)
        {
            if (transformations.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemCommand command)
        {
            context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}