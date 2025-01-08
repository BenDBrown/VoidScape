extends Area2D
class_name player_entered_trigger
signal player_entered
signal player_exited

func _ready() -> void:
	for i in get_parent().get_children():
		if i is Comet_tracker:
			connect("player_entered",i.set_allowed_to_spawn.bind("false"))
			connect("player_exited",i.set_allowed_to_spawn.bind("true"))

func _on_body_entered(body: Node2D) -> void:
	if body == Game.PlayerShip:
		emit_signal("player_entered")

func _on_body_exited(body: Node2D) -> void:
	if body == Game.PlayerShip:
		emit_signal("player_exited")
