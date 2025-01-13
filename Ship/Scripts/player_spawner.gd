extends Node2D
class_name PlayerSpawner
@export var player_ship_scene: PackedScene = preload("res://Ship/Prefabs/PlayerShip.tscn")
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
	gameOver.hide()
	inventory.hide()
	get_parent().add_child(playerShip)
	get_parent().add_child(hud)
	get_parent().add_child(gameOver)
	get_parent().add_child(inventory)
	saver.build_ship(playerShip)
	playerShip.TryBuildShip()
	playerShip.global_position = global_position
	playerShip.OnDestroyed.connect(on_destroyed)

func on_destroyed(ship):
	hud.hide()
	gameOver.show()
