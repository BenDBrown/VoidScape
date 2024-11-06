extends Node2D

class_name ItemLister
signal added_child(child: Node2D)
const COMPONENT_COPY_AMOUNT: int = 3
const SHIP_DATA_DIR_PATH = "res://Resources/ShipData/"
@onready var draggable_scene = preload("res://Ship/ShipBuilder/DraggableComponent.tscn")

func display_items():
	# Open the directory
	var dir = DirAccess.open(SHIP_DATA_DIR_PATH)

	# Iterate through all files in the directory
	dir.list_dir_begin()
	var fileName = dir.get_next()
	var x: int = 0
	var y: int = 0

	while fileName != "":
		# Check if the file is a scene file (you may want to customize this check)
		if !fileName.ends_with(".tres"):
			break

		for i in COMPONENT_COPY_AMOUNT:
			var draggable = draggable_scene.instantiate() as DraggableComponent
			added_child.emit(draggable)
			var compData = ResourceLoader.load(SHIP_DATA_DIR_PATH + fileName) as ShipComponentData
			var component = compData.GetPrefab() as ShipComponent
			component.Data = compData
			draggable.add_child(component)
			draggable.shipComponent = component
			draggable.global_position = Vector2(global_position.x + x * 34, global_position.y + y * 34)
			if x == 1:
				y += 1
			x = (x + 1) % 2
			draggable.SetStartPosition(draggable.global_position)
		fileName = dir.get_next()
	dir.list_dir_end()
