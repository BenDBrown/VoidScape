extends Resource
class_name ShipSaver

const PATH = "res://Resources/PlayerShip.tres"

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
		var thruster = scene.instantiate() as Thruster;
		thruster.SetPowerDraw(dict[pos].powerDraw)
		thruster.SetThrust(dict[pos].thrust)
		parent.add_child(thruster)
		thruster.position = pos*32
