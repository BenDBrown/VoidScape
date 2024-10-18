extends Node

@onready var save_data

@onready var player_ship = "playership"
var fx: AudioStreamPlayer2D
const SAVE_PATH = "res://saves/"

func _init() -> void:
	fx = AudioStreamPlayer2D.new()
	add_child(fx)

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

func play_fx(sound, time: float = 0):
	fx.stream = sound
	fx.play(time)
