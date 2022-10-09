using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game
{
    public static class Constants
    {
        public static readonly string GlobalCreaturesSchedulerEvent = "Global_Creatures";

        public static readonly int GlobalCreaturesSchedulerEventInterval = 1000;

        public static readonly string GlobalItemsSchedulerEvent = "Global_Items";

        public static readonly int GlobalItemsSchedulerEventInterval = 60000;

        public static readonly string GlobalLightSchedulerEvent = "Global_Light";

        public static readonly int GlobalLightSchedulerEventInterval = Clock.Interval;

        public static string CreatureWalkSchedulerEvent(Creature creature) => "Creature_Walk_" + creature.Id;

        public static string CreatureTalkSchedulerEvent(Creature creature) => "Creature_Talk_" + creature.Id;

        public static readonly int CreatureTalkSchedulerEventInterval = 30000;

        public static string ItemDecaySchedulerEvent(Item item) => "Item_Decay_" + item.Id;

        public static string PlayerActionSchedulerEvent(Player player) => "Player_Action_" + player.Id;

        public static readonly int PlayerActionSchedulerEventInterval = 200;

        public static string PlayerCheckConnectionSchedulerEvent(Player player) => "Player_Check_Connection_" + player.Id;

        public static readonly int PlayerCheckConnectionSchedulerEventInterval = 60000;


        public static readonly string OnlyProtocol86Allowed = "Only protocol 8.6 allowed.";

        public static readonly string AccountNameOrPasswordIsNotCorrect = "Account name or password is not correct.";

        public static readonly string SorryNotPossible = "Sorry, not possible.";

        public static readonly string TargetLost = "Target lost.";

        public static readonly string ThereIsNotEnoughtRoom = "There is not enought room.";

        public static readonly string ThereIsNoWay = "There is no way.";

        public static readonly string ThisIsImpossible = "This is impossible.";

        public static readonly string YouCanNotMoveThisObject = "You cannot move this object.";

        public static readonly string YouCannotPutMoreObjectsInThisContainer = "You cannot put more objects in this container.";

        public static readonly string YouCanNotTakeThisObject = "You cannot take this object.";

        public static readonly string YouCanNotThrowThere = "You cannot throw there.";

        public static readonly string YouCanNotUseThere = "You cannot use there.";

        public static readonly string YouCanNotUseThisItem = "You cannot use this item.";

        public static readonly string YouMayNotAttackThisCreature = "You may not attack this creature.";


        public static readonly int ObjectsPerPoint = 10;
    }
}