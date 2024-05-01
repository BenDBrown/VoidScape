extends Node

@onready var save_data

func set_save_data(data):
	save_data = data;

func get_save_data():
	var gold = 10;
	
	#Change this to add stuff to the save file
	var json: Dictionary = {
		"Gold": gold
	}
	var jstr = JSON.stringify(json);
	return jstr;

func on_enemy_death():
	pass

func _process(_delta):
	if Input.is_key_pressed(KEY_F4):
		get_tree().quit();
