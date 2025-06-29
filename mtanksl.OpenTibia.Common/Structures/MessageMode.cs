namespace OpenTibia.Common.Structures
{
    public enum MessageMode : byte
    {
    None = 0,

        Say = 1, // TalkType.Say, TextColor.YellowDefault

        Whisper = 2, // TalkType.Whisper, TextColor.YellowDefault

        Yell = 3, // TalkType.Yell, TextColor.YellowDefault

        PrivateFrom = 4, // TalkType.Private, TextColor.TealDefault

        PrivateTo = 5,

        ChannelManagement = 6, // TalkType.ChannelWhite

        Channel = 7, // TalkType.ChannelYellow

        ChannelHighlight = 8, // TalkType.ChannelOrange

        Spell = 9,

        NpcTo = 10, // TalkType.PrivatePlayerToNpc, TextColor.PurpleDefault

        NpcFrom = 11, // TalkType.PrivateNpcToPlayer, TextColor.TealDefaultAndNpcs

        GamemasterBroadcast = 12, // TalkType.Broadcast, TextColor.RedServerLog

        GamemasterChannel = 13, // TalkType.ChannelRed

        GamemasterPrivateFrom = 14, // TalkType.PrivateRed, TextColor.RedServerLog

        GamemasterPrivateTo = 15,

        Login = 16,

        Warning = 17, // TextColor.RedCenterGameWindowAndServerLog

        Game = 18, // TextColor.WhiteCenterGameWindowAndServerLog

        Failure = 19, // TextColor.WhiteBottomGameWindow

        Look = 20, // TextColor.GreenCenterGameWindowAndServerLog

        DamageDealed = 21,

        DamageReceived = 22,

        Heal = 23,

        Exp = 24,

        DamageOthers = 25,

        HealOthers = 26,

        ExpOthers = 27,

        Status = 28, // TextColor.WhiteBottomGameWindowAndServerLog

        Loot = 29,

        TradeNpc = 30,

        Guild = 31,

        PartyManagement = 32,

        Party = 33,

        BarkLow = 34,

        BarkLoud = 35,

        Report = 36,

        HotkeyUse = 37,

        TutorialHint = 38,

        Thankyou = 39,

        Market = 40,

        Mana = 41,

    BeyondLast = 42,

        MonsterYell = 43, // TalkType.MonsterYell, TextColor.OrangeDefault

        MonsterSay = 44, // TalkType.MonsterSay, TextColor.OrangeDefault

        Red = 45,

        Blue = 46,

        RVRChannel = 47, // TalkType.ReportRuleViolationOpen

        RVRAnswer = 48, // TalkType.ReportRuleViolationAnswer

        RVRContinue = 49, // TalkType.ReportRuleViolationQuestion

        GameHighlight = 50,

        NpcFromStartBlock = 51, 

    LastMessage = 52,

        Unknown = 53, // TalkType.Unknown

        GamemasterChannelAnonymous = 54 // TalkType.ChannelRedAnonymous
    }
}