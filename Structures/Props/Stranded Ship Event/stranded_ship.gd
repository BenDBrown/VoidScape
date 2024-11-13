extends Node2D

var sos_signal_range = []
var dialogue_range = []

func _on_sos_signal_range_body_entered(body: Node2D) -> void:
	
	var entered_body = body.get_parent()
	if entered_body is Game.PlayerShip and entered_body not in sos_signal_range:
		sos_signal_range.append(entered_body)
		print("Incoming Emergency Transmission: To anyone that can hear me, I have been ambushed and stranded. Please help me out.")


func _on_dialogue_range_body_entered(body: Node2D) -> void:
	var entered_body = body.get_parent()
	if entered_body is Game.PlayerShip and entered_body not in dialogue_range:
		dialogue_range.append(entered_body)
		print("Hey, I need help pls.")
