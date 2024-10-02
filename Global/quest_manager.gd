extends Node

@export var questLimit = 10
@export var quests = []

func try_add_quest(quest):
	if quests.size() >= questLimit:
		return false
	quests.append(quest)
	return true

func get_quests():
	return quests

func check(target):
	for quest in quests:
		quest.check_target(target)
