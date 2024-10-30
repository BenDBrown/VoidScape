extends Node2D

@export var player_ship_scene: PackedScene = preload("res://Ship/Prefabs/PlayerShip.tscn")

func _ready():
	var playerShip = player_ship_scene.instantiate() as Ship
	var saver = PlayerShipSave.new()
	saver = saver.load_save()
	get_parent().add_child.call_deferred(playerShip)
	saver.build_ship(playerShip)
	playerShip.TryBuildShip()
	playerShip.global_position = global_position
	Game.player_ship = playerShip
