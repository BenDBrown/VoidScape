extends Resource

class_name Saveable

func save():
	if !DirAccess.dir_exists_absolute(Game.SAVE_PATH):
		DirAccess.make_dir_absolute(Game.SAVE_PATH)
	var err = ResourceSaver.save(self, Game.SAVE_PATH + get_save_name()) as Error
	if err != OK:
		printerr(get_save_name() + ": " + error_string(err))

func load_save() -> Saveable:
	if FileAccess.file_exists(Game.SAVE_PATH + get_save_name()):
		return ResourceLoader.load(Game.SAVE_PATH + get_save_name())
	else:
		return self

func delete_save() -> bool:
	if FileAccess.file_exists(Game.SAVE_PATH + get_save_name()):
		var dir = DirAccess.open(Game.SAVE_PATH)
		return dir.remove(get_save_name()) == OK
	return false

func get_save_name():
	pass
