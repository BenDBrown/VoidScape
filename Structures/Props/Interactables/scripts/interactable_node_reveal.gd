extends Node2D
signal send_to_manager
signal change_interact_ui_visability
var visability = true
var reward_given
@export var node_to_reveal:PackedScene
@export var location_of_node:Vector2
@export var manager:interactable_manager
@export var credit_reward = 0
@export var cargo_reward:Cargo
var ps:PlayerShip

func _ready() -> void:
	connect("send_to_manager",manager.send_to_manager_node_reveal.bind(node_to_reveal,location_of_node,self))
	connect("change_interact_ui_visability",manager.interact_ui_visablity.bind(visability))
	Game.PlayerShip.connect("InteractableInteracted",send)



func _on_area_2d_body_entered(body: Node2D) -> void:
	visability = true
	if body == Game.PlayerShip:
		if !reward_given:
			emit_signal("change_interact_ui_visability")
			Game.PlayerShip.CanInteract(true)

func _on_area_2d_body_exited(body: Node2D) -> void:
	visability = false
	if body == Game.PlayerShip:
		Game.PlayerShip.CanInteract(false)
		emit_signal("change_interact_ui_visability")
	

func send():
	if !reward_given:
		emit_signal("send_to_manager")
		reward_given = true
