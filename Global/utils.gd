extends Node

const SAVE_PATH = "res://save.bin"

func save_game():
	var file = FileAccess.open(SAVE_PATH, FileAccess.WRITE)
	file.store_line(Game.get_save_data());
	
func load_game():
	if !FileAccess.file_exists(SAVE_PATH):
		return false;
	
	var file = FileAccess.open(SAVE_PATH, FileAccess.READ)
	var data = JSON.parse_string(file);
	Game.set_save_data(data);
	return true;
