extends Resource
class_name ShipSaver

const PATH = "Ship.tres"

@export var ship: Dictionary = {}

func add_component(pos, component: ShipComponent):
	if ship.has(pos): return
	var info = component.GetInfo() as Dictionary
	info["path"] = component.scene_file_path
	ship[pos] = info

func save():
	if FileAccess.file_exists(Game.SAVE_PATH+PATH):
		DirAccess.remove_absolute(PATH)
	else:
		var dir = DirAccess.open("res://")
		dir.make_dir("Resources")
	ResourceSaver.save(self, PATH)

func build_ship(parent:Node2D):
	for pos in ship.keys():
		var scene = load(ship[pos].path) as PackedScene;
		var component = scene.instantiate() as ShipComponent;
		component.SetInfo(ship[pos])
		parent.add_child(component)
		component.position = pos*32

static func load_save()->Resource:
	if !FileAccess.file_exists(PATH):
		print("Ship file doesn't exist")
		return null;
	return ResourceLoader.load(PATH)
