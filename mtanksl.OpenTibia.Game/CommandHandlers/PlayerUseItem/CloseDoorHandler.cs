using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CloseDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 5100, 5099 },
            { 5102, 5101 },
            { 5109, 5108 },
            { 5111, 5110 },
                  
            { 1211, 1210 },
            { 1214, 1213 },
            { 1220, 1219 },
            { 1222, 1221 },
            { 5139, 5138 },
            { 5142, 5141 },
                  
            { 1233, 1232 },
            { 1236, 1235 },
            { 1238, 1237 },
            { 1240, 1239 },
                  
            { 1251, 1250 },
            { 1254, 1253 },
            { 5516, 5515 },
            { 5518, 5517 },
                 
            { 5118, 5117 },
            { 5120, 5119 },
            { 5127, 5126 },
            { 5129, 5128 },
            { 5136, 5135 },
            { 5145, 5144 },

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