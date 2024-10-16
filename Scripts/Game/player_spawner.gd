extends Node2D

var player_ship_scene = preload("res://Prefabs/PlayerShip.tscn")

func _ready():
	var playerShip = player_ship_scene.instantiate() as PlayerShip
	var saver = ShipSaver.new()
	saver = saver.load_save()
	get_parent().add_child.call_deferred(playerShip)
	saver.build_ship(playerShip)
	playerShip.TryBuildShip()
	playerShip.global_position = global_position
