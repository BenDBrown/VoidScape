extends Node2D

var detected = []
var detected2 = []

func _on_sos_signal_range_body_entered(body: Node2D) -> void:
	
	var entered_body = body.get_parent()
	if entered_body is Ship and entered_body not in detected:
		detected.append(entered_body)
		print("Incoming Emergency Transmission: To anyone that can hear me, I have been ambushed and stranded. Please help me out.")


func _on_dialogue_range_body_entered(body: Node2D) -> void:
	var entered_body = body.get_parent()
	if entered_body is Ship and entered_body not in detected2:
		detected2.append(entered_body)
		print("Hey, I need help pls.")
