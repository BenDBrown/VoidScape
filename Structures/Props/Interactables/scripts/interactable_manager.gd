extends Node2D
class_name interactable_manager
var interact_ui:Control

func send_to_manager_map_reveal(map_location, _raduis_of_reveal):
	pass
	#ToDo add the money system from Ismet

func send_to_manager_node_reveal(node_to_reveal:PackedScene, location_of_node:Vector2, interactable):
	var node = node_to_reveal.instantiate()
	interactable.add_child(node)
	node.global_position = location_of_node

func interact_ui_visibility_true():
	find_hud()
	interact_ui.set_visible(true)

func interact_ui_visibility_false():
	find_hud()
	interact_ui.set_visible(false)


func find_hud():
	for c in Game.Hud.get_children():
		if c is Interactable_ui:
			interact_ui = c
