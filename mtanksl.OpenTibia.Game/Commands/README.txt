ParseTurnCommand 
	-> CreatureUpdateDirectionCommand (+ Scripts)

ParseWalkCommand 
	-> CreatureUpdateParentCommand (+ Scripts) 
		-> TileRemoveCreatureEvent 
		-> TileAddCreatureEvent

ParseMoveItemFromTileToTileCommand 
	-> PlayerMoveCreatureCommand (+ Scripts) 
		-> CreatureUpdateParentCommand (+ Scripts) 
			-> TileRemoveCreatureEvent 
			-> TileAddCreatureEvent

ParseMoveItemFromTileToTileCommand 
	-> PlayerMoveItemCommand (+ Scripts) 
		-> ItemUpdateParentToTileCommand (+ Scripts) 
			-> TileRemoveItemCommand 
				-> TileRemoveItemEvent 
			-> TileAddItemCommand 
				-> TileAddItemEvent