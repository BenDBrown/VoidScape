extends Node2D

class_name ItemDropComponent
@export var drops: Array[Cargo]

func drop_loot():
	for loot in drops:
		loot.call_deferred("spawn", get_tree().current_scene, global_position)
