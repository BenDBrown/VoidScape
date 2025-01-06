extends Area2D
class_name player_entered_trigger
signal player_entered
signal player_exited

func _on_body_entered(body: Node2D) -> void:
	if body == Game.PlayerShip:
		player_entered.emit

func _on_body_exited(body: Node2D) -> void:
	if body == Game.PlayerShip:
		player_exited.emit
