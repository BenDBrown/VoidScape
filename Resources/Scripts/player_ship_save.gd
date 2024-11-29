extends Saveable
class_name PlayerShipSave

const NAME = "Ship.tres"

@export var ship: Dictionary = {}

func add_components(components):
	clear()
	for pos in components:
		add_component(pos, components[pos])

func add_component(pos, component: ShipComponent):
	if ship.has(pos):
		return
	var shipComponent: Dictionary
	shipComponent["component"] = {"Mirrored": component.IsMirrored, "LocalRotation": component.rotation,}
	shipComponent["path"] = component.Data.GetPrefabPath()
	shipComponent["data_path"] = component.Data.resource_path
	ship[pos] = shipComponent

func build_ship(parent: Node2D):
	if !ship:
		load_save()
	for pos in ship.keys():

		var scene = load(ship[pos].path) as PackedScene;
		var component = scene.instantiate() as ShipComponent;
		if parent is Ship:
			parent.AddComponent(component, pos)
		else:
			parent.add_child(component)
			component.position = pos * 32
		component.IsMirrored = ship[pos]["component"]["Mirrored"] as bool
		component.rotation = ship[pos]["component"]["LocalRotation"]
		if ship[pos].has("data_path"):
			var data_path = ship[pos]["data_path"]
			component.SetupData(ResourceLoader.load(data_path) as ShipComponentData)

func clear():
	ship = {}

func get_save_name():
	return NAME
