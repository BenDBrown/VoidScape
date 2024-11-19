extends Node2D

@export var player_ship_scene: PackedScene = preload("res://Ship/Prefabs/PlayerShip.tscn")

func _ready():
	var saver = PlayerShipSave.new()
	saver = saver.load_save()

	var playerShip = player_ship_scene.instantiate() as Ship
	get_parent().add_child.call_deferred(playerShip)
	saver.build_ship(playerShip)
	playerShip.TryBuildShip()
	playerShip.global_position = global_position
	Game.PlayerShip = playerShip
