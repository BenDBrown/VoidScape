extends Resource
class_name ShipSaver

const PATH = "res://Resources/Ship.tres"

@export var dict: Dictionary = {}

func add_component(pos, component: ShipComponent):
	if dict.has(pos): return
	var info = component.GetInfo()
	info["path"] = component.scene_file_path
	dict[pos] = info

func save():
	if FileAccess.file_exists(PATH):
		DirAccess.remove_absolute(PATH)
	ResourceSaver.save(self, PATH)

func create(parent:Node2D):
	for pos in dict.keys():
		var scene = load(dict[pos].path) as PackedScene;
		var component = scene.instantiate() as ShipComponent;
		component.SetInfo(dict[pos])
		parent.add_child(component)
		component.position = pos*32
