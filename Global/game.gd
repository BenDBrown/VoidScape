extends Node

@onready var save_data
@onready var player_ship = preload("res://Prefabs/PlayerShip.tscn")
const SAVE_PATH = "res://saves/"
#TODO REMOVE WHEN LIVE
func _process(_delta):
	if !OS.has_feature("debug"):
		return
	if Input.is_key_pressed(KEY_F4):
		get_tree().quit()
	if Input.is_key_pressed(KEY_ESCAPE):
		get_tree().change_scene_to_file("res://Scenes/start_menu.tscn")

func build_ship():
	pass

func save_game():
	pass

func load_game():
	pass
