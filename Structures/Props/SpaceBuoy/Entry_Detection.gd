extends Area2D


var detected = []

func _on_body_entered(body: Node2D) -> void:
	
	var entered_body = body.get_parent()
	if entered_body is Ship and entered_body not in detected:
		
		# TODO: Should check whether the player ship has the right to access
		
		detected.append(entered_body)
		print("ACCESS DENIED: NO TRAVEL PASS.")
		print("GO BACK OR ACCEPT DEATH.")
	

func _on_body_exited(body: Node2D) -> void:
	
	var entered_body = body.get_parent()
	if entered_body is Ship and entered_body in detected:
		detected.erase(entered_body)
