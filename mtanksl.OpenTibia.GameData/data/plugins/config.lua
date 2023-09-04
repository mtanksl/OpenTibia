plugins = {
  actions = {
    { type = "PlayerRotateItem", opentibiaid = 1740, filename = "rotate.lua" },
    { type = "PlayerUseItem", opentibiaid = 1740, filename = "use.lua" },
  },
  talkactions = {
    { type = "PlayerSay", message = "hello world", filename = "say.lua" },
  },
  npcs = {
    { type = "Conversation", name = "Al Dee", filename = "al dee.lua" },
    { type = "Conversation", name = "Rachel", filename = "rachel.lua" },
    { type = "Conversation", name = "Cipfried", filename = "cipfried.lua" },
    { type = "Conversation", name = "Captain Bluebear", filename = "captain bluebear.lua" }
  }
}