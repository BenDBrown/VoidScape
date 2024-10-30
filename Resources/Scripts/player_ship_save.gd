extends Saveable
class_name PlayerShipSave

const NAME = "Ship.tres"

@export var ship: Dictionary = {}

func add_component(pos, component: ShipComponent):
	if ship.has(pos): return
	var shipComponent: Dictionary

	shipComponent["component"] = component
	shipComponent["path"] = component.scene_file_path
	ship[pos] = shipComponent

func build_ship(parent: Node2D):
	if !ship:
		load_save()
	for pos in ship.keys():
		var scene = load(ship[pos].path) as PackedScene;
		var component = scene.instantiate() as ShipComponent;
		parent.add_child(component)
		component.position = pos * 32

func get_save_name():
	return NAME
