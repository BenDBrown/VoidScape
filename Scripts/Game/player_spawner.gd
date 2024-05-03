extends Node

@onready var parent = $Node2D
var saver:ShipSaver = ShipSaver.new()
var player_ship_scene = preload("res://Prefabs/PlayerShip.tscn")
func _ready():
	var playerShip = player_ship_scene.instantiate()
	get_parent().add_child(playerShip)
	saver.create(playerShip)
	queue_free()
