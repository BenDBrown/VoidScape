extends Node2D
class_name  enemy_outpost
signal  player_entered_outpost
signal player_exited_outpost



func _on_area_2d_body_entered(body: Node2D) -> void:
	emit_signal("player_entered_outpost")


func _on_area_2d_body_exited(body: Node2D) -> void:
	emit_signal("player_exited_outpost")
