@icon("res://Assets/Other/Quest Icon.png")

extends Node
class_name Quest

signal completed;
signal delivered;

enum State{UNSELECTED, ONGOING, COMPLETED, DELIVERED}

@export var state: State = State.UNSELECTED;
@export var quest_name: String;
@export_multiline var quest_description = "";

func _ready():
	QuestManager.try_add_quest(self);
	
func get_data()->String:
	var data = {}
	for objective in self.get_children():
		data[objective.objective_name] = objective.objective_description;
	return JSON.stringify(data);

func check_target(target):
	for objective in get_children():
		objective.is_target(target);
