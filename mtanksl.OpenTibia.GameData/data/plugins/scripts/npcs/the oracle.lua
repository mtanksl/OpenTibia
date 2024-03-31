local say = topic:new()
local topic1 = topic:new(say)
local topic2 = topic:new(say)
local topic3 = topic:new(say)

say:add("", function(module, npc, player, message, captures, parameters)
                module.farewell()
            end)
say:add("yes", "In which town do you want to leave: Carlin, Thais or Venore?", { topic = topic1 } )

topic1:add("thais", "In Thais! And what profession have you choosen: Knight, Paladin, Sorcerer ou Druid?", { city = "Thais", topic = topic2 } )
topic1:add("carlin", "In Carlin! And what profession have you choosen: Knight, Paladin, Sorcerer ou Druid?", { city = "Carlin", topic = topic2 } )
topic1:add("venore", "In Venore! And what profession have you choosen: Knight, Paladin, Sorcerer ou Druid?", { city = "Venore", topic = topic2 } )
topic1:add("", "Carlin, Thais or Venore?")

topic2:add("knight", "A Knight! Are you sure? This decision is irreversible!", { profession = "Knight", topic = topic3 } )
topic2:add("paladin", "A Paladin! Are you sure? This decision is irreversible!", { profession = "Paladin", topic = topic3 } )
topic2:add("sorcerer", "A Sorcerer! Are you sure? This decision is irreversible!", { profession = "Sorcerer", topic = topic3 } )
topic2:add("druid", "A Druid! Are you sure? This decision is irreversible!", { profession = "Druid", topic = topic3 } )
topic2:add("", "Knight, Paladin, Sorcerer ou Druid?")

topic3:add("yes", function(module, npc, player, message, captures, parameters)
                      module.say("So be it!")
                      module.idle()
                      --TODO
                  end)

local handler = npchandler:new( {
    greet = "[playername], are you prepared to face your destiny?",
    busy = "Wait until it is your turn!",
    say = say,
    farewell = "Come back when you are prepared to face your destiny!",
    disappear = "Come back when you are prepared to face your destiny!"
} )

registernpcsdialogue("The Oracle", handler)