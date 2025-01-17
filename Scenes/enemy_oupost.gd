extends Node2D
class_name  enemy_outpost
signal  player_entered_outpost
signal player_exited_outpost



func _on_area_2d_body_entered(_body: Node2D) -> void:
	player_entered_outpost.emit()


func _on_area_2d_body_exited(_body: Node2D) -> void:
	player_exited_outpost.emit()
