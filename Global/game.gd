extends Node

@onready var save_data
@onready var player_ship = "playership"

func _process(_delta):
	if Input.is_key_pressed(KEY_F4):
		get_tree().quit()

func build_ship():
	pass
