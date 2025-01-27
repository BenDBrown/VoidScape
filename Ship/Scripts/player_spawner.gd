extends Node2D
class_name PlayerSpawner
var player_ship_scene: PackedScene = preload("res://Ship/Prefabs/PlayerShip.tscn")
@export var hud_scene: PackedScene = preload("res://UI/Scenes/HUD.tscn")
@export var game_over_scene: PackedScene = preload("res://UI/Scenes/Game Over.tscn")
@export var inventory_scene: PackedScene = preload("res://UI/Scenes/Inventory.tscn")

var saver: PlayerShipSave
var hud
var gameOver
var inventory

func _ready():
	saver = PlayerShipSave.new()
	saver = saver.load_save()
	var playerShip = player_ship_scene.instantiate() as Ship
	Game.PlayerShip = playerShip
	call_deferred("deferred", playerShip);

func deferred(playerShip:PlayerShip):
	hud = hud_scene.instantiate()
	gameOver = game_over_scene.instantiate()
	inventory = inventory_scene.instantiate()
	Game.Hud = hud
	get_parent().add_child(playerShip)
	get_parent().add_child(hud)
	get_parent().add_child(gameOver)
	get_parent().add_child(inventory)
	saver.build_ship(playerShip)
	playerShip.TryBuildShip()
	set_spawn(playerShip)
	playerShip.OnDestroyed.connect(on_destroyed)

func on_destroyed(ship):
	hud.hide()
	gameOver.show()

func set_spawn(ship: PlayerShip):
	var spawnPointSave = SpawnPointSave.new()
	spawnPointSave = spawnPointSave.load_save()
	if(spawnPointSave.spawn_point != Vector2.ZERO):
		ship.global_position = spawnPointSave.spawn_point
	else:
		ship.global_position = global_position
