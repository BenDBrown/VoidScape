extends Node2D

class_name LootDropComponent
@export var loot_table: LootTable
@export var guaranteed_drops: Array[Loot]

func drop_loot():
	for loot in guaranteed_drops:

		loot.call_deferred("spawn",get_tree().current_scene, global_position)
	if loot_table:
		loot_table.spawn()
