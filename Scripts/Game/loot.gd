@icon("res://Assets/Other/Loot Icon.png")
extends Resource
class_name Loot
var lootScene = preload("res://Prefabs/Loot/loot.tscn")
@export var sprite:Texture2D
@export var cargo: Cargo

func spawn(sceneTree, globalPosition: Vector2):
	var lootDrop = lootScene.instantiate() as LootDrop
	sceneTree.add_child(lootDrop)
	lootDrop.set_sprite(sprite)
	lootDrop.cargo = cargo
	lootDrop.global_position = globalPosition
