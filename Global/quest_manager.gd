extends Node

@export var questLimit = 10
@export var quests = []

func try_add_quest(quest):
	if quests.size() >= questLimit:
		return false;
	quests.append(quest);
	
func on_death(ship):
	pass

