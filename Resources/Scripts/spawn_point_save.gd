extends Saveable
class_name SpawnPointSave

const NAME = "SpawnPoint.tres"
@export var spawn_point : Vector2

func get_save_name():
	return NAME
	
