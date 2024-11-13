extends Node2D

class_name ItemLister
signal added_child(child: Node2D)
const COMPONENT_COPY_AMOUNT: int = 3

@export var datas : Array[Resource]
@onready var draggable_scene = preload("res://Ship/ShipBuilder/DraggableComponent.tscn")

func display_items():
	var x = 0
	var y = 0
	for data in datas:
		for i in COMPONENT_COPY_AMOUNT:
			var draggable = draggable_scene.instantiate() as DraggableComponent
			added_child.emit(draggable)
			var component = data.GetPrefab() as ShipComponent
			component.Data = data
			draggable.add_child(component)
			draggable.shipComponent = component
			draggable.global_position = Vector2(global_position.x + x * 34, global_position.y + y * 34)
			draggable.SetStartPosition(draggable.global_position)
			x+=1
			if x == 9:
				y+=1
				x = 0
