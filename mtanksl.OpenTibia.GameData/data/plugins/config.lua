plugins = {
  actions = {
    { type = "PlayerRotateItem", opentibiaid = 1740, filename = "rotate.lua" },
    { type = "PlayerUseItem", opentibiaid = 1740, filename = "use.lua" },
  },
  talkactions = {
    { type = "PlayerSay", message = "hello world", filename = "say.lua" },
  },
  npcs = {
    { type = "Dialogue", name = "Al Dee", filename = "al dee.lua" },
    { type = "Dialogue", name = "Rachel", filename = "rachel.lua" },
    { type = "Dialogue", name = "Cipfried", filename = "cipfried.lua" },
    { type = "Dialogue", name = "Captain Bluebear", filename = "captain bluebear.lua" }
  }
}