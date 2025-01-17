extends Area2D
class_name player_entered_trigger
signal player_entered
signal player_exited

func _ready() -> void:
	for i in get_tree().current_scene.get_children():
		if i is Comet_tracker:
			player_entered.connect(i.set_allowed_to_spawn.bind("false"))
			player_exited.connect(i.set_allowed_to_spawn.bind("true"))

func _on_body_entered(body: Node2D) -> void:
	if body == Game.PlayerShip:
		player_entered.emit()

func _on_body_exited(body: Node2D) -> void:
	if body == Game.PlayerShip:
		player_exited.emit()
