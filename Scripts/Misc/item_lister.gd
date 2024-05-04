extends Node2D
@onready var draggableScene = preload("res://Prefabs/ShipBuilder/DraggableComponent.tscn")
var comp_path = "res://Prefabs/ShipComponents/"
@onready var builder = $"../Builder"
func _ready():
	# Open the directory
	var dir = DirAccess.open(comp_path)

	# Iterate through all files in the directory
	dir.list_dir_begin()
	var file_name = dir.get_next()
	var x: int = 0;
	var y: int = 0;
	while file_name != "":
		# Check if the file is a scene file (you may want to customize this check)
		if !file_name.ends_with(".tscn"): break;
		# Load the scene
		var scene = load(comp_path + file_name)
		if scene == null: break;
		var draggable = draggableScene.instantiate() as Draggable
		var component = scene.instantiate() as ShipComponent
		
		builder.add_child(draggable)
		draggable.add_child(component)
		draggable.shipComponent = component
		draggable.global_position = Vector2(global_position.x+x*34,global_position.y+y*34)
		if x == 1:
			y+=1
		x = (x+1)%2
		
		file_name = dir.get_next()
	dir.list_dir_end()
