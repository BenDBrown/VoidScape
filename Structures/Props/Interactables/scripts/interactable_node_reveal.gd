extends Node2D
signal send_to_manager
signal change_interact_ui_visibility_true
signal change_interact_ui_visibility_false
var reward_given
@export var node_to_reveal:PackedScene
@export var location_of_node:Vector2
@export var manager:interactable_manager
@export var credit_reward = 0
@export var cargo_reward:Cargo

func _ready() -> void:
	send_to_manager.connect(manager.send_to_manager_node_reveal.bind(node_to_reveal,location_of_node,self))
	change_interact_ui_visibility_true.connect(manager.interact_ui_visibility_true)
	change_interact_ui_visibility_false.connect(manager.interact_ui_visibility_false)




func _on_area_2d_body_entered(body: Node2D) -> void:
	if body == Game.PlayerShip:
		if !reward_given:
			Game.PlayerShip.InteractableInteracted.connect(send)
			change_interact_ui_visibility_true.emit()


func _on_area_2d_body_exited(body: Node2D) -> void:
	if body == Game.PlayerShip:
		Game.PlayerShip.InteractableInteracted.disconnect(send)
		change_interact_ui_visibility_false.emit()


func send():
	if !reward_given:
		send_to_manager.emit()
		reward_given = true
