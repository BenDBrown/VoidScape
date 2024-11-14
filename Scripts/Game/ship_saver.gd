extends Saveable
class_name ShipSaver

const NAME = "Ship.tres"

@export var ship: Dictionary = {}

func add_component(pos, component: ShipComponent):
	if ship.has(pos): return
	var info = component.GetInfo() as Dictionary
	info["path"] = component.scene_file_path
	ship[pos] = info

func build_ship(parent:Node2D):
	for pos in ship.keys():
		var scene = load(ship[pos].path) as PackedScene;
		var component = scene.instantiate() as ShipComponent;
		component.SetInfo(ship[pos])
		parent.add_child(component)
		component.position = pos*32

func get_save_name():
	return NAME
