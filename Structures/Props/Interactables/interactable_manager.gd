extends Node2D
class_name interactable_manager
var interact_ui:Control

func _ready() -> void:
	pass
	

func send_to_manager_map_reveal(map_location, _raduis_of_reveal):
	pass
	#ToDo add the money system from Ismet

func send_to_manager_node_reveal(node_to_reveal:PackedScene, location_of_node:Vector2):
	var node = node_to_reveal.instantiate()
	get_parent().add_child(node)
	node.global_position = location_of_node

func interact_ui_visablity(visability):
	find_hud()
	interact_ui.set_visible(visability)


func find_hud():
	for c in get_parent().get_children():
		if c is CanvasLayer:
			if c.name == "HUD":
				for ui in c.get_children():
					if ui is Interactable_ui:
						interact_ui = ui
