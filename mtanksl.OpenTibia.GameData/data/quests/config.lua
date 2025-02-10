-- If the player has storage key 1 with value 0, the mission 1 is displayed in the quest's dialog
-- If the player has storage key 2 with value 0, the mission 2 is displayed in the quest's dialog
-- If the player has storage key 3 with value 0, the mission 3 is displayed in the quest's dialog
-- If the player has storage key 1, 2 or 3, the quest 1 is displayed in the quest's dialog
-- If the player has the quest 1 displayed in the quest's dialog but with no mission left, it is shown as completed

quests = {
	{ id = 1, name = "Quest 1", missions = {
		{ name = "Mission 1", description = "Description 1", storagekey = 1, storagevalue = 0 },
		{ name = "Mission 2", description = "Description 2", storagekey = 2, storagevalue = 0 },
		{ name = "Mission 3", description = "Description 3", storagekey = 3, storagevalue = 0 }
	} }
}