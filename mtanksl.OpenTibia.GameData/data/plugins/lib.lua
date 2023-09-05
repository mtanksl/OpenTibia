-- Functions

-- void print(params object[] paramaters)
-- void delay(int seconds)
-- string delay(int seconds, Action callback)
-- void delaygameobject(GameObject gameObject, int seconds)
-- string delaygameobject(GameObject gameObject, int seconds, Action callback)
-- bool canceldelay(string key)
-- void creaturewalk(Creature creature, Tile tile)
-- void creatureupdatedirection(Creature creature, Direction direction)
-- void creatureupdatehealth(Creature creature, int health)
-- void creatureupdateinvisible(Creature creature, bool invisible)
-- void creatureupdatelight(Creature creature, Light light)
-- void creatureupdateoutfit(Creature creature, Outfit baseOutfit, Outfit outfit)
-- void creatureupdatepartyicon(Creature creature, PartyIcon partyIcon)
-- void creatureupdateskullicon(Creature creature, SkullIcon skullIcon)
-- void creatureupdatespeed(Creature creature, ushort baseSpeed, ushort speed)
-- void showanimatedtext(Position position, AnimatedTextColor animatedTextColor, string message)
-- void showmagiceffect(Position position, MagicEffectType magicEffectType)
-- void showprojectile(Position fromPosition, Position toPosition, ProjectileType projectileType)
-- void showtext(Creature creature, TalkType talkType, string message)
-- void showwindowtext(Player player, TextColor textColor, string message)
-- void fluiditemupdatefluidtype(FluidItem fluidItem, FluidType fluidType)
-- void itemdestroy(Item item)
-- Item itemtransform(Item item, ushort openTibiaId, byte count)
-- void monsterdestroy(Monster monster)
-- void monstersay(Monster monster, string message)
-- void npcdestroy(Npc npc)
-- void npcsay(Npc npc, string message)
-- void npcaddmoney(Player player, int price)
-- void npcdeletemoney(Player player, int price)
-- int npccountmoney(Player player)
-- void npcadditem(Player player, ushort openTibiaId, byte type, int count)
-- void npcremoveitem(Player player, ushort openTibiaId, byte type, int count)
-- int npccountitem(Player player, ushort openTibiaId, byte type)
-- void playerdestroy(Player player)
-- void playerupdatecapacity(Player player, int capacity)
-- void playerupdateexperience(Player player, int experience, ushort level, byte levelPercent)
-- void playerupdatemana(Player player, int mana)
-- void playerupdatesoul(Player player, int soul)
-- void playerupdatestamina(Player player, int stamina)
-- (bool, int) playergetstorage(Player player, int key)
-- void playersetstorage(Player player, int key, int value)
-- void splashitemupdatefluidtype(SplashItem splashItem, FluidType fluidType)
-- void stackableitemupdatecount(StackableItem stackableItem, byte count)

-- Enums

addon = {
    none = 0,
    first = 1,
    second = 2,
    both = 3
}

animatedTextColor = {
  blue = 5,
  green = 30,
  lightblue = 35,
  crystal = 65,
  purple = 83,
  platinum = 89,
  lightgrey = 129,
  darkred = 144,
  red = 180,
  orange = 198,
  gold = 210,
  white = 215
}

direction = {
	north = 0,
	east = 1,
	south = 2,
	west = 3
}

fluidtype = {
	empty = 0,
	water = 1,
	blood = 2,
	beer = 3,
	slime = 4,
	lemonade = 5,
	milk = 6,
	manafluid = 7,
	lifefluid = 10,
	oil = 11,
	urine = 13,
	coconutmilk = 14,
	wine = 15,
	mud = 19,
	fruitjuice = 21,
	lava = 26,
	rum = 27
}

magiceffecttype = {
	redspark = 1,
	bluerings = 2,
	puff = 3,
	yellowspark = 4,
	explosionarea = 5,
	explosiondamage = 6,
	firearea = 7,
	yellowrings = 8,
	greenrings = 9,
	blackspark = 10,
	teleport = 11,
	energydamage = 12,
	blueshimmer = 13,
	redshimmer = 14,
	greenshimmer = 15,
	fireplume = 16,
	greenspark = 17,
	mortarea = 18,
	greennotes = 19,
	rednotes = 20,
	poisonarea = 21,
	yellownotes = 22,
	purplenotes = 23,
	bluenotes = 24,
	whitenotes = 25,
	bubbles = 26,
	dice = 27,
	giftwraps = 28,
	fireworkyellow = 29,
	fireworkred = 30,
	fireworkblue = 31,
	stun = 32,
	sleep = 33,
	watercreature = 34,
	groundshaker = 35,
	hearts = 36,
	fireattack = 37,
	energyarea = 38,
	smallclouds = 39,
	holydamage = 40,
	bigclouds = 41,
	icearea = 42,
	icetornado = 43,
	iceattack = 44,
	stones = 45,
	smallplants = 46,
	carniphilia = 47,
	purpleenergy = 48,
	yellowenergy = 49,
	holyarea = 50,
	bigplants = 51,
	cake = 52,
	giantice = 53,
	watersplash = 54,
	plantattack = 55,
	tutorialarrow = 56,
	tutorialsquare = 57,
	mirrorhorizontal = 58,
	mirrorvertical = 59,
	skullhorizontal = 60,
	skullvertical = 61,
	assassin = 62,
	stepshorizontal = 63,
	bloodysteps = 64,
	stepsvertical = 65,
	yalaharighost = 66,
	bats = 67,
	smoke = 68,
	insects = 69,
	dragonhead = 70
}

partyicon = {
	none = 0,
	whiteyellow = 1,
	whiteblue = 2,
	blue = 3,
	yellow = 4,
	bluesharedexperience = 5,
	yellowsharedexperience = 6,
	bluenosharedexperienceblink = 7,
	yellownosharedexperienceblink = 8,
	bluenosharedexperience = 9,
	yellownosharedexperience = 10,
	gray = 11
}

projectiletype = {
	spear = 1,
	bolt = 2,
	arrow = 3,
	fire = 4,
	energy = 5,
	poisonarrow = 6,
	burstarrow = 7,
	throwingstar = 8,
	throwingknife = 9,
	smallstone = 10,
	skull = 11,
	bigstone = 12,
	snowball = 13,
	powerbolt = 14,
	poison = 15,
	infernalbolt = 16,
	huntingspear = 17,
	enchantedspear = 18,
	assassinstar = 19,
	viperstar = 20,
	royalspear = 21,
	sniperarrow = 22,
	onyxarrow = 23,
	piercingbolt = 24,
	whirlwindsword = 25,
	whirlwindaxe = 26,
	whirlwindclub = 27,
	ethernalspear = 28,
	ice = 29,
	earth = 30,
	holy = 31,
	suddendeath = 32,
	flasharrow = 33,
	flamingarrow = 34,
	shiverarrow = 35,
	energysmall = 36,
	icesmall = 37,
	holysmall = 38,
	earthsmall = 39,
	eartharrow = 40,
	explosion = 41,
	cake = 42
}

skullicon = {
	none = 0,
	yellow = 1,
	green = 2,
	white = 3,
	red = 4,
	black = 5
}

talktype = {
	say = 1,
	whisper = 2,
	yell = 3,
	privateplayertonpc = 4,
	privatenpctoplayer = 5,
	private = 6, 
	channelyellow = 7,
	channelwhite = 8,
	reportruleviolationopen = 9,
	reportruleviolationanswer = 10,
	reportruleviolationquestion = 11,
	broadcast = 12,
	channelred = 13,
	privatered = 14,
	channelorange = 15,
	channelredanonymous = 17,
	monstersay = 19,
	monsteryell = 20
}

textcolor = {
	yellowdefault = 1,
	purpledefault = 4,
	tealdefaultandnpcs = 5,
	tealdefault = 6,
	redserverlog = 12,
	reddefault = 16,
	orangedefault = 19,
	redcentergamewindowandserverlog = 21,
	whitecentergamewindowandserverlog = 22,
	whitebottomgamewindowandserverlog = 23,
	greencentergamewindowandserverlog = 25,
	whitebottomgamewindow = 26
}

conditionspecialcondition = {
	none = 0,
	poisoned = 1,
	burning = 2,
	electrified = 4,
	drunk = 8,
	magicshield = 16,
	slowed = 32,
	haste = 64,
	logoutblock = 128,
	drowning = 256,
	freezing = 512,
	dazzled = 1024,
	cursed = 2048,
	bleeding = 4096,
	outfit = 32768,
	invisible = 65536,
	light = 131072,
	regeneration = 262144,
	soul = 524288,
	muted = 1048576
}