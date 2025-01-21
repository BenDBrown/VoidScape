extends Node
@export var manager: interactable_manager
@export var text: Label
var timer: Timer
func _on_area_2d_body_entered(body: Node2D) -> void:
	if body == Game.PlayerShip:
		manager.interact_ui_visibility_true()
		Game.PlayerShip.InteractableInteracted.connect(interact)


func _on_area_2d_body_exited(body: Node2D) -> void:
	if body == Game.PlayerShip:
		manager.interact_ui_visibility_false()
		Game.PlayerShip.InteractableInteracted.disconnect(interact)

func interact():
	print("trigger")
	if !timer:
		timer = Timer.new()
		add_child(timer)
		timer.timeout.connect(text.hide)
	if timer.is_stopped() || timer.paused:
		text.show()
		timer.start(2)
