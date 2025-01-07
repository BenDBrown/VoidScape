extends Node

@export var cargo: Cargo

func _on_parent_destroyed(ship: CharacterBody2D) -> void:
	if cargo:
		cargo.Spawn(get_tree().current_scene, ship.global_position)
