# What is this?

It is an open Tibia server, a simple one, written in C# from scratch. 

# Why another one?

I think one can only learn by doing. 

# Notes before proceding

Please dont take this too seriously ;)
This project is really old, I did it as a tribute to the open Tibia community. 
From time to time, I take this project to have some fun and recall the good old days.

# First run

You need Visual Studio and .NET 4.6.1.
Set mtanksl.OpenTibia.Host as startup project.
Press F5 to run the server. 
Use Tibia 8.6 client (download below) with an IP Changer (also download below) to connect to localhost (IP address 127.0.0.1) on port 7171.
Use account number 1 and password 1 to enter game.

![Server and Client](/server.png)

# Tibia 8.6 client

You can download the official release direct from here [4Shared](https://www.4shared.com/s/fVTbjUnjCiq).

# IP Changer 

You can use [jo3bingham's IP Changer](https://github.com/jo3bingham/tibia-ip-changer), [OtLand IP Changer](https://otland.net/threads/otland-ip-changer.134369/) or download direct from here [4Shared](https://www.4shared.com/s/f2VQahgxIiq).

# Map editor

You can use [Remere's map editor](https://github.com/hampusborgos/rme).

# What is done?

A lot of the elementary stuff is implemented, such as Tibia and Open Tibia's file format interpreters (.dat, .otb, .otbm, .pic, .spr), TCP socket management for login and game servers, packets and communication protocol (with RSA, Xtea, Adler32), task scheduler thread, main game dispatcher thread, the base objects, the base structures, etc. 

I tried to implement the base Tibia's game mechanics on top of that. Controlling the character (walking, turning, changing outfit, etc), interacting with other players (say, whisper, yell, direct chat, channels, private channel, rule violation channel, etc), interacting with the game world (look item, move item, rotate item, use item, use item with creature, use item with item, etc).

But there is still a lot to be done.

# How it works?

The server architecture is the following: *Packets* are the primary unity. *Incoming packets* wrap the intent of the client and *Outgoing packets* wrap all the information of a server change that the client needs to receive in order to keep in sync. *Commands* abstract away how and which clients needs to receive this information. Commands can be intercepted and changed as they pass throw a pipeline of *Command Handlers* before executing. Commands may generate events, which can be listened by *Event Handlers*. Before starting the server, all the classes that implement *IScript* are loaded. This is when the pipeline order is set. Every game object (item, monster, npc and player) can have some *Behaviour* attached to it. Behaviour is like Unity's Component. 

# In-game command list

- /a `n` - Jumps n tiles
- /up - Goes up one floor
- /down - Goes down one floor
- /t `town_name` - Goes to town
- /w `waypoint_name` - Goes to waypont
- /goto `player_name` - Goes to player
- /c `player_name` - Teleports player
- /i `item_id` - Creates an item
- /m `monster_name` - Creates a monster
- /n `npc_name` - Creates a NPC
- /r - Destroys a monster or a NPC
- /kick `player_name` - Kicks player

# Spells list
- exani tera - Support
- exani hur up - Support
- exani hur down - Support
- utevo lux - Light
- utevo gran lux - Light
- utevo vis lux - Light
- utani hur - Speed
- utani gran hur - Speed
- exura - Healing
- exura gran - Healing
- exura vita - Healing
- exura gran mas res - Healing
- exori mort - Attack
- exori flam - Attack
- exori vis - Attack
- exevo flam hur - Attack
- exevo vis lux - Attack
- exevo gran vis lux - Attack
- exevo mort hur - Attack
- exevo gran mas vis - Attack
- exevo gran mas pox - Attack
- Exori - Attack

Notes: Formulas are based on Tibia Wiki and RealOTS. Not perfect, but it serves as an inspiration for now.

# Runes list
- Poison field
- Poison bomb
- Poison wall
- Fire field
- Fire bomb
- Fire wall
- Energy field
- Energy bomb
- Energy wall
- Fireball
- Great fireball
- Explosion
- Magic wall
- Wild growth
- Intense healing
- Ultimate healing
- Light magic missile
- Heavy magic missile
- Sudden death

Notes: Formulas are based on Tibia Wiki and RealOTS. Not perfect, but it serves as an inspiration for now.
