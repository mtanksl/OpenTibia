# To do

- Player information
	- Capacity
	- Soul regeneration
	- Item that modifies player light (ex. torch)
	- Item that modifies player speed (ex. BOH)
	- Item that modifies player skills
- Fight	
	- vs Player
	- PVP vs non-PVP configuration
	- Logout block
	- Magic level and skills
	- Skull
	- War
	- Splash
	- Persist loss
	- Remove experience and skills on player death
	- Drunk and Slowed conditions
	- ...
- Bed
- Guild
- Hangable items
- Map clean up
- Raids
- Rings and amulets charges
- ...

# Done (or good enough for now)

- Tibia and Open Tibia's file format interpreters (.dat, .otb, .otbm, .pic, .spr)
- TCP socket management for login and game servers
- Packets and communication protocol (with RSA, Xtea, Adler32)
- Task scheduler thread
- Main game dispatcher thread
- The base objects
	- Vocation (knight, paladin, sorcerer, druid, etc) and rank (player, tutor, gamemaster) 
- The base structures
- Server status info protocol
- Interacting with the client
	- Premium days
	- Message of the day
	- Waiting list
	- Quests
		- Database for storage
	- Achievements
		- Database for storing achievements
	- Hotkeys
	- Report bug
	- Report rule violation
	- Debug assert
- Controlling the character
	- Login
	- Walking
	- Turning
	- Changing outfit
		- Database for storing outfits/addons
	- Logout
- Interacting with other players
	- Say
		- Gamemaster in-game commands
		- Spells
			- Database for storing spells
	- Whisper
	- Yell
	- Direct chat
	- Channels
	- Private channel
	- Rule violation channel
	- VIP
		- Database for storing VIPs
	- Safe Trade
	- Party
- Interacting with npc
	- Dialogue
	- Buy and sell items
	- Trade window
	- Travel
- Interacting with monsters
	- Fight
		- Monster spawn/respawn	
		- Monster despawn
		- Loot
		- Experience, level and level percent
		- Drop bag
		- Drop items
		- AOL
		- Bless
			- Database for storing blesses
	- Weapon attributes (range, atk, def, arm)
	- Ammunition
	- Bow and arrow
	- Wand and rod
	- Two-handed items
	- Protection zone
	- No-logout zone
- Interacting with the game world 
	- Look item
		- Sign items
		- House doors
	- Move item
	- Rotate item
	- Use item
		- Containers
		- Lockers and towns
		- Read and write items
		- Quest chest item
		- etc
	- Use item with creature
		- Runes
		- etc
	- Use item with item
		- Tools
		- etc
	- Mail
		- Send parcel
		- Send letter
	- House
		- Access list
		- House items
- Plugins
	- Lua scripting
	- Lua debugging with ZeroBrane
	- Lua autocomplete intellisense
- Security
	- Ban/unban by player name, account or ip address
	- Rate limiting
		- Kick multi-client
		- Connections abuse
		- Packets abuse
		- Login attempts abuse
		- Kick slow sockets
		- Invalid message abuse
		- Unknown packet abuse	
	- Stop accepting new connections once limit is reached
	- Kick after 1 minute without pong response
	- Kick after 15 minutes idle
	- Maintenance info
	- Anti-spam: channel mute
- Server statistics
	- Active connections
	- #Received messages and bytes
	- #Sent messages and bytes
	- Average processing time