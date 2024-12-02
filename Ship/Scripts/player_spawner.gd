extends Node2D

@export var player_ship_scene: PackedScene = preload("res://Ship/Prefabs/PlayerShip.tscn")
@export var hud_scene: PackedScene = preload("res://UI/Scenes/HUD.tscn")

var saver

func _ready():
	saver = PlayerShipSave.new()
	saver = saver.load_save()
	var playerShip = player_ship_scene.instantiate() as Ship
	Game.PlayerShip = playerShip
	call_deferred("deferred", playerShip);

func deferred(playerShip):
	var hud = hud_scene.instantiate()
	get_parent().add_child(playerShip)
	get_parent().add_child(hud)
	saver.build_ship(playerShip)
	playerShip.TryBuildShip()
	playerShip.global_position = global_position
