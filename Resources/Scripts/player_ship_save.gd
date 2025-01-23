extends Saveable
class_name PlayerShipSave

const NAME = "Ship.tres"
const PRESET_SHIP = "res://Resources/Presets/Ship.tres"

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
	shipComponent["colour"] = component.Colour
	ship[pos] = shipComponent

func build_ship(parent: Node2D):
	for pos in ship.keys():

		var scene = load(ship[pos].path) as PackedScene;
		var component = scene.instantiate() as ShipComponent;
		if parent is Ship:
			parent.AddComponent(component, pos)
		else:
			parent.add_child(component)
			component.position = pos * 32
		if(ship[pos]["component"]["Mirrored"] as bool):
			component.Mirror()
		var rotations = ship[pos]["component"]["LocalRotation"]/90
		var rotatingRight = rotations >= 0
		for i in abs(rotations):
			if(rotatingRight):
				component.RotateRight()
			else:
				component.RotateLeft()
		component.SetColour(ship[pos]["colour"])
		if ship[pos].has("data_path"):
			var data_path = ship[pos]["data_path"]
			component.SetupData(ResourceLoader.load(data_path) as ShipComponentData)

func clear():
	ship = {}

func load_save():
	var result = super.load_save()
	if result == self:
		print("self")
		return ResourceLoader.load(PRESET_SHIP)
	return result

func get_save_name():
	return NAME
