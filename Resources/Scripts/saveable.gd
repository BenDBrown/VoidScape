extends Resource

class_name Saveable

func save():
	if !DirAccess.dir_exists_absolute(Game.SAVE_PATH):
		DirAccess.make_dir_absolute(Game.SAVE_PATH)
	var err = ResourceSaver.save(self, Game.SAVE_PATH + get_save_name())
	if err != 0:
		printerr(err)

func load_save():
	if FileAccess.file_exists(Game.SAVE_PATH+ get_save_name()):
		return ResourceLoader.load(Game.SAVE_PATH + get_save_name())
	else:
		return self

func get_save_name():
	pass
