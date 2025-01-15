extends Node

@export var on_exit = false
@export var cargo: Resource
@export var loot_scene = preload("res://Misc/Cargo/loot_drop.tscn")

func drop() -> void:
	var lootDrop = loot_scene.instantiate() as LootDrop
	call_deferred("defer",lootDrop)


func defer(lootDrop):
	get_tree().current_scene.add_child(lootDrop)
	lootDrop.global_position = get_parent().global_position
	lootDrop.cargo = cargo
	if cargo is Cargo:
		lootDrop.set_sprite(cargo.sprite)
	if cargo is ShipComponentData:
		lootDrop.set_sprite(cargo.Sprite)

func _exit_tree() -> void:
	if on_exit:
		drop()
